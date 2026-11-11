using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public GameObject mainPanel;
    [SerializeField] public GameObject settingsPanel;
    [SerializeField] public GameObject characterPanel;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void Play()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        characterPanel.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Settings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
        characterPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        mainPanel.SetActive(true);
        characterPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}
