using System;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    /*------This is for the panel choosing to end game or just respawn. This is just UI and refrences game manager, so if 
     * any more buttons are added, you'll refrence functions called in gameManager
     */
    [Header("Panels")]
    public GameObject gameOverPanel;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    //button links functions
    public void OnRespawnClicked()
    {
        // Hide the UI
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        gameManager.Respawn();
    }

    public void OnRestartClicked()
    {
        // Hide the UI
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        gameManager.RestartLevel();
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
