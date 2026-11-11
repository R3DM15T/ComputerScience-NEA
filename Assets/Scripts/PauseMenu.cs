using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject pauseMenuBaseUI;
    public GameObject Player;
    public GameObject settingsMenuUI;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        
    }


    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseMenuBaseUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void LoadMenu()
    {
       SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit(); //close the game
    }

    public void LoadSettings()
    {
        settingsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void GoBack()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        pauseMenuBaseUI.SetActive(true);
    }
}
