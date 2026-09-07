using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuPanel;

    [Header("Shared Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    private GameObject lastMenuPanel;
    private bool isPaused;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        Time.timeScale = 0f;
        isPaused = false;
    }

    private void Update()
    {
        if (mainMenuPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void PlayGame()
    {
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void OpenSettings()
    {
        lastMenuPanel = mainMenuPanel.activeSelf ? mainMenuPanel : pauseMenuPanel;
        lastMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        lastMenuPanel = mainMenuPanel.activeSelf ? mainMenuPanel : pauseMenuPanel;
        lastMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void BackToPreviousMenu()
    {
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        lastMenuPanel.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}