using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public float HealthPecent => currentHealth / maxHealth;
    public bool IsAlive => currentHealth > 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Kill();
        }

    }

    public void Kill()
    {
        currentHealth = 0;
        //all other kill criteria here, UI calls, VFX, sound, etc
    }

    //private void Update()
   // {
      //  if (Input.GetKeyDown(KeyCode.Space))
      //  {
       //     TakeDamage(10);
       // }
    //}

}
