using Platformer.Player;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    /*------This is basic player health manager. When things damage player, they call the scripts in here.
     * It also handles when you die, and redirects to the game manager for the UI and resetting the game. Checkpoints
     * are handeled in their own script and gameManager
     */

    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public float HealthPecent => currentHealth / maxHealth;
    public bool IsAlive => currentHealth > 0;
    private GameManager gameManager;

    void Start()
    {
        currentHealth = maxHealth;
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        //prevents player from moving and stops it from falling
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        // animator.SetTrigger("Die"); // Add this later

        currentHealth = maxHealth;
        //now to move onto UI inside gamemanager script
        gameManager.DieProcess();
    }

    /*-----Memphis's code below------
    //private void Update()
   // {
      //  if (Input.GetKeyDown(KeyCode.Space))
      //  {
       //     TakeDamage(10);
       // }
    //}
    */

}
