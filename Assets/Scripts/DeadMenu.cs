using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{

    float delay = 1f;
    public GameObject deadMenu;
    public int gameStartScene2;

    // Start is called before the first frame update
    void Start()
    {
        delay = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("time " + Time.timeScale);
        Debug.Log("fixed " + Time.fixedDeltaTime);
        if(Break.exploded)
        {
            delay -= Time.deltaTime;
            if(delay <= 0f)
            {
                dead();
            }
        }
    }

    public void dead()
    {
        deadMenu.SetActive(true);
        Time.timeScale = 0.2f;
    }

    public void restart()
    {
        //Time.timeScale = 1f;
        //SceneManager.LoadScene(gameStartScene2);
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        Time.timeScale = 1;
        //PlayerMovement.movement.enabled = false;
        //PlayerMovement.movement.enabled = true;

    }
}
