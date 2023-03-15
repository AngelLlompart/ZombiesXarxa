using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int enemiesAlive;
    public int round;

    private GameObject[] spawnPoints;

    public GameObject enemyPrefab;

    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI surviveRoundsText;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Animator fadeAnimator;
    private GameObject _weapon;
    private GameObject _player;
    private bool paused = false;
    private bool gameOver = false;
    private MouseLook _mouseLook;

    private bool fadeOut = false;
    // Start is called before the first frame update
    void Start()
    {
        InitLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesAlive == 0)
        {
            NextWave(++round);
            roundText.text = "Round: " + round;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if (paused)
            {
                UnPause();
            }
            else
            {
                Pause();
            }
        }
    }

    private void NextWave(int round)
    {
        for (int i = 0; i < round; i++)
        {
            int spawnerRandom = Random.Range(0, 7);
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[spawnerRandom].transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyManager>().gameManager = GetComponent<GameManager>();
            enemiesAlive++;
        }
    }

    private void Pause()
    {
        StopTime();
        paused = true;
        pauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        _mouseLook.enabled = true;
        _weapon.GetComponent<WeaponManager>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        paused = false;
    }

    public void Menu()
    {
        fadeOut = true;
        //UnPause();
        fadeAnimator.SetTrigger("fadein");
        StartCoroutine(DelayMenu());
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
    public void Quit()
    {
        #if UNITY_EDITOR
                if(EditorApplication.isPlaying) 
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
        #else
                Application.Quit();
        #endif
    }

    public void GameOver()
    {
        StopTime();
        gameOver = true;
        surviveRoundsText.text = "Rounds Sruvived \n" + round;
        gameOverMenu.SetActive(true);
        
    }

    private void StopTime()
    {
        Time.timeScale = 0;
        _mouseLook.enabled = false;
        _weapon.GetComponent<WeaponManager>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void InitLevel()
    {
        enemiesAlive = 0;
        round = 0;
        gameOver = false;
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");
        _player = GameObject.FindWithTag("Player");
        _weapon = GameObject.Find("weapon");
        gameOverMenu.SetActive(false);
        /*foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);   
        }*/
        _mouseLook = _player.GetComponent<MouseLook>();
        _player.GetComponent<PlayerManager>().RestartHP();
        UnPause();
    }

    IEnumerator DelayMenu()
    {
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene("MainMenu");
    }
}
