using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    public string tutorialSceneName = "Tutorial";
    public string level1SceneName = "Nivel_1";

    public void PlayFromTutorial()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }

    public void PlayFromLevel1()
    {
        SceneManager.LoadScene(level1SceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
