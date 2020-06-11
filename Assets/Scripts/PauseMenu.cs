using UnityEngine;
public class PauseMenu : MonoBehaviour
{
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
    // Ma 2 funkcje, w zaleznosci czy gra trwa czy się zakończyła. Gdy gra jest zakończona
    // to z menu pauzy znika przycisk resume i pojawia się tekst o zaliczeniu poziomu.
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
    // Powrot do Menu
    public void Exit() 
    {
        Time.timeScale = 1f; 
        Menu.main.Return(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Menu.main.Return(true);
        isGamePaused = false;

    }
}
