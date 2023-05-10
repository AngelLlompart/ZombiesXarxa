using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
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
    //private GameObject _weapon;
    private GameObject _player;
    public bool paused = false;
    public bool gameOver = false;
    private MouseLook _mouseLook;

    private bool fadeOut = false;
    public PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        InitLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.InRoom || (PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            if (enemiesAlive == 0)
            {
                NextWave(++round);
                if (PhotonNetwork.InRoom)
                {
                    Hashtable hash = new Hashtable();
                    hash.Add("currentRound", round);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                }
                else
                {
                    DisplayNextRound(round);
                }
            }
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

    private void DisplayNextRound(int round)
    {
        roundText.text = "Round: " + round;
    }
    
    private void NextWave(int round)
    {
        for (int i = 0; i < round; i++)
        {
            int spawnerRandom = Random.Range(0, 7);
            GameObject enemy;
            if (PhotonNetwork.InRoom)
            {
                enemy = PhotonNetwork.Instantiate("Zombie 1", spawnPoints[spawnerRandom].transform.position,
                    Quaternion.identity);
            }
            else
            {
                enemy = Instantiate(Resources.Load("Zombie 1"), spawnPoints[spawnerRandom].transform.position, Quaternion.identity) as GameObject;
            }
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
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 1;
        }
        pauseMenu.SetActive(false);
        _mouseLook.enabled = true;
        _player.GetComponent<PlayerMovement2>().enabled = true;
        //_weapon.GetComponent<WeaponManager>().enabled = true;
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
        if (PhotonNetwork.InRoom)
        {
            SceneManager.LoadScene("GameOnline");  
        }
        else
        {
            SceneManager.LoadScene("Game");  
        }
       
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

    public void StopTime()
    { 
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 0;
        }
        _mouseLook.enabled = false;
        //_player.GetComponent<PlayerMovement2>().enabled = false;
        //_weapon.GetComponent<WeaponManager>().enabled = false;
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
        //_weapon = GameObject.Find("weapon");
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
        /*if (PhotonNetwork.InRoom)
        {
            SceneManager.LoadScene("MultiplayerScene");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }*/
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (photonView.IsMine)
        {
            if (changedProps["currentRound"] != null)
            {
                DisplayNextRound((int) changedProps["currentRound"]);
            }
        }
    }
}
