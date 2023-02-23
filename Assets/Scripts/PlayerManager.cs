using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public float health = 100;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject camera;
    private Quaternion posInitCam;

    private GameManager _gameManager;
    [SerializeField] private GameObject hitPanel;

    private float shakeTime = 0.5f;

    private float shakeDuration = 1;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        healthText.text = "Health: " + health;
        posInitCam = camera.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(float dmg)
    {
        StartCoroutine(CameraMove());
        health -= dmg;
        if (health <= 0)
        {
            _gameManager.GameOver();
        }

        healthText.text = "Health: " + health;
    }

    public void RestartHP()
    {
        health = 100;
        healthText.text = "Health: " + health;
    }
    
    IEnumerator CameraMove()
    {
        hitPanel.SetActive(true);
        camera.transform.localRotation = Quaternion.Euler(Random.Range(-4f, 4f), 0, 0);
        yield return new WaitForSecondsRealtime(0.4f);
        hitPanel.SetActive(false);
        camera.transform.localRotation = posInitCam;
    }
}
