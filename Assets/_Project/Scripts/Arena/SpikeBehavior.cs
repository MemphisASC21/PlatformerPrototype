using UnityEngine;
using System.Collections;

public class SpikeBehavior : MonoBehaviour
{
    /*----------LOGIC---------------
     * Basically here, I just reversed the Thwomp script. The logic is the same so that the player doesn't get hurt
     * if they run into the edge of the spikes, but the tip of the spikes will deal damage (just like the thwomp)
     */
    private enum State { Idle, Attacking, Retracting, Cooldown }
    private State currentState = State.Idle;

    [Header("Spike Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackSpeed = 15f;   
    [SerializeField] private float retractSpeed = 3f;   
    [SerializeField] private float spikeHeight = 1f;  
    [SerializeField] private float holdTime = 1f;      
    [SerializeField] private float cooldownTime = 0.5f; 

    private Vector3 startPos;
    private bool isPlayerNear = false;

    void Start()
    {
        startPos = transform.position;
    }
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (currentState == State.Idle)
            {
                StartCoroutine(SpikeRoutine());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    //health
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            Debug.Log("Player collided with thwomp");
            playerHealth.TakeDamage(damage);
        }
    }
    private IEnumerator SpikeRoutine()
    {
        currentState = State.Attacking;
        Vector3 targetPos = startPos + (Vector3.up * spikeHeight);

        //move up
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, attackSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        //wait so its not too fast
        yield return new WaitForSeconds(holdTime);

        //go down
        currentState = State.Retracting;
        while (Vector3.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, retractSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = startPos;

        //cooldown (so it doesn't repeat too fast)
        currentState = State.Cooldown;
        yield return new WaitForSeconds(cooldownTime);

        //repeat if they are stil there
        if (isPlayerNear)
        {
            StartCoroutine(SpikeRoutine());
        }
        else
        {
            currentState = State.Idle;
        }
    }
}
