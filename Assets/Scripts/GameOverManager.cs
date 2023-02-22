using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
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
}
