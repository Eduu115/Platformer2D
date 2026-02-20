using UnityEngine;

public class PauseButton : MonoBehaviour
{
    private PauseManager pauseManager;

    void Start()
    {
        pauseManager = FindObjectOfType<PauseManager>();
    }

    public void OnPauseButtonClicked()
    {
        pauseManager.PauseGame();
    }
}