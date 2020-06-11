using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject resumeButton;
    public GameObject finishedText;
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                Resume();
            else
                Pause(false);
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    public void Pause(bool gameOver)
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;

        if(gameOver)
        {
            resumeButton.SetActive(false);
            finishedText.SetActive(true);
        }
    }
    public void Exit() // Powrot do Menu
    {
        Time.timeScale = 1f; // Must have, inaczej gra sie pauzuje
        isGamePaused = false;
        Menu.main.Return(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        Menu.main.Return(true);
    }
}
