using Platformer.Player;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    /*-----Why this script------
     * We need the GameManager to easily hold events like checkpoints,
     * player dealth, and respawns. Without this, values and events get messy being hardcoded
     * in each script. Here will we find respawn, death, checkpoint functions that also handle said UI events for those.
     */

    [Header("UI")]
    public GameObject gameOverPanel;
    public int playerCheckpointIs { get; private set; }
    private bool isGameOver = false;
    private Vector3 respawnPoint;


    void Start()
    {
        gameOverPanel.SetActive(false);
        playerCheckpointIs = 0;
    }

    public void UpdateCheckpoint(int newCheckPointNum, Vector3 newPosition)
    {
        if (newCheckPointNum > playerCheckpointIs)
        {
            playerCheckpointIs = newCheckPointNum;
            respawnPoint = newPosition;
            Debug.Log("Checkpoint Updated to: " + playerCheckpointIs);
        }
    }

    public void DieProcess()
    {
        //this is esentially a null check, to make sure we don't end the game if the player isn't dead
        if (isGameOver) return;
        isGameOver = true;

        //UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void Respawn()
    {
        //reset gameover
        isGameOver = false;
        //find the player because we'll have to reset all of the conditions on the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = respawnPoint;
            player.GetComponent<PlayerController>().enabled = true;
            //player.GetComponent<PlayerHealth>().ResetHealth(); //reset player health
        }
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }
}
