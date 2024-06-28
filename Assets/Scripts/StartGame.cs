using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public int gameStartScene;

    public void StartGames()
    {

        SceneManager.LoadScene(gameStartScene);
        
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
