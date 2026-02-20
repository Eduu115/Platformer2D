using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuPanel;

    [Header("Buttons")]
    public Button resumeButton;
    public Button restartLevelButton;
    public Button restartFromLevel1Button;
    public Button exitButton;

    [Header("Scene Names")]
    public string tutorialSceneName = "Tutorial";
    public string level1SceneName = "Nivel1";
    public string mainMenuSceneName = "MainMenu"; // o el nombre que tengas

    private bool isPaused = false;

    void Start()
    {
        // Asegurarse de que el menú empieza oculto
        pauseMenuPanel.SetActive(false);

        // Asignar funciones a los botones
        resumeButton.onClick.AddListener(ResumeGame);
        restartLevelButton.onClick.AddListener(RestartLevel);
        restartFromLevel1Button.onClick.AddListener(RestartFromLevel1);
        exitButton.onClick.AddListener(ExitGame);
    }

    void Update()
    {
        // Pausar / reanudar con Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;  // Congela el juego
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;  // Reanuda el juego
        isPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartFromLevel1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(level1SceneName);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;

        // Si tienes menú principal, carga esa escena
        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
        else
            Application.Quit(); // Solo funciona en build, no en editor
    }
}