using System;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    /*------This is for the panel choosing to end game or just respawn. This is just UI and refrences game manager, so if 
     * any more buttons are added, you'll refrence functions called in gameManager
     */
    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject winPanel;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        //THIS MAY CAUSE A BUG! Because it is nested, if gameOverUIPanel fails or is null, win UI won't run. It should work
        //but if ywe get bugs lets check this out. Not sure if nested if statements are good here. 
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            if (winPanel != null)
            {
                winPanel.SetActive(false);
            }
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void ShowWinScreen()
    {
        if (winPanel != null) winPanel.SetActive(true);
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
