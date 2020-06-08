using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                Resume();
            else
                Pause();
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }
    public void Exit() // Powrot do Menu
    {
        throw new NotImplementedException();
        //zla praktyka, ale nie ma znaczenia w tym przypadku  
        SceneManager.LoadScene("Menu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Wczytuje aktywna scene
        Time.timeScale = 1f; // Must have, inaczej gra sie pauzuje
    }
}
