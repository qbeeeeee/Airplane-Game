using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    public GameObject menuPause;
    public static bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        menuPause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!Break.exploded){
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

    }

    public void Pause()
    {
        PlayerMovement.engine.enabled = false;
        menuPause.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        PlayerMovement.engine.enabled = true;
        menuPause.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Quit()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
