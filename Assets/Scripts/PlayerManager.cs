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
    [SerializeField] private CanvasGroup hitPanel;

    private float shakeTime = 1f;

    private float shakeDuration = 0.3f;
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
        if(shakeTime < shakeDuration)
        {
            
            hitPanel.alpha = 1;
            
            shakeTime += Time.deltaTime;
            CameraShake();
        }else {
            camera.transform.localRotation = posInitCam;
            if (hitPanel.alpha > 0)
            {
                hitPanel.alpha -= Time.deltaTime;
            }

        }

    }

    public void Hit(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            _gameManager.GameOver();
        }
        else
        {
            shakeTime = 0;
        }
        

        healthText.text = "Health: " + health;
    }

    public void RestartHP()
    {
        health = 100;
        healthText.text = "Health: " + health;
    }
    
    private void CameraShake()
    {
        camera.transform.localRotation = Quaternion.Euler(Random.Range(-2f, 2f), 0, 0);
        
    }
}
