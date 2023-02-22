using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public float health = 100;

    [SerializeField] private TextMeshProUGUI healthText;
    // Start is called before the first frame update
    void Start()
    {
        healthText.text = "Health: " + health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        healthText.text = "Health: " + health;
    }

    public void RestartHP()
    {
        health = 100;
        healthText.text = "Health: " + health;
    }
}
