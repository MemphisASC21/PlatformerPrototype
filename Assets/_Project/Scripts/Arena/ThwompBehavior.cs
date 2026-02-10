using Platformer.Player;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ThwompBehavior : MonoBehaviour
{
    /*------------------------------ABOUT THE SCRIPT------------------------
     * This is for the falling blocks. What it does is fall and rise at variable times (set in inspector). We can make fast
     * and slow falling blocks this way. It sends a raycast down to detetct the ground to determine how far it "falls".*/

    private enum State { Idle, Falling, Rising, Cooldown }
    private State currentState = State.Idle;
    //--FOR HEALTH SCRIPT, look at how vamp survivors did it-- private PlayerHealth _playerHealth;

    [Header("Thwomp Settings")]
    //[SerializeField] private float fallAcceleration = 20f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float fallSpeed = 12f;
    [SerializeField] private float riseSpeed = 5f;
    [SerializeField] private float bottomWaitTime = 1f;
    [SerializeField] private float topWaitTime = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    private Vector3 startPos;
    private Rigidbody2D rb;
    private bool isPlayerUnder = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerUnder = true;

            if (currentState == State.Idle)
            {
                StartCoroutine(FallRoutine());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerUnder = false;
        }
    }

    /* ------Health to add in when we set that up----------
     * also, make sure that the health is ONLY subtracted when the state is falling. That way, if the player bumps into it while rising
     * they don't die, same with cooldown (ex. they jump and hit the bottom of it.)
     */
     private void OnCollisionEnter2D(Collision2D collision)
     {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            //Debug.Log("Player collided with thwomp");
            playerHealth.TakeDamage(damage);
        }
     }

    private IEnumerator FallRoutine()
    {
        currentState = State.Falling;
        // Raycast strictly to ground layer (ignores player/enemies)
        /*--------------------ZANE TO DO: ----------------------------
         * INSERT CODE TO CHECK DIMENSIONS OF THWOMP, BOX CAST DOWN, NEAREST GROUND IS HOW FAR IT MOVES
         * THIS WAY WE CAN MAKE HIDEY SPOTS UNDER A THWOMP FOR PLAYER TO HIDE. AT TOP GET VARIABLES OF THE COLLIDER AND SEND A CAST DOWN
         * 
         */
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 50f, groundLayer);
        if (hit.collider == null)
        {
            currentState = State.Idle;
            yield break; 
        }
        float targetY = hit.point.y + (transform.localScale.y / 2f);
        //fall
        while (transform.position.y > targetY)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, 0), fallSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, targetY, 0);
        currentState = State.Cooldown;
        yield return new WaitForSeconds(bottomWaitTime);
        currentState = State.Rising;

        while (Vector2.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, riseSpeed * Time.deltaTime);
            yield return null;
        }
        
        transform.position = startPos;

        if (isPlayerUnder)
        {
            currentState = State.Cooldown;
            yield return new WaitForSeconds(topWaitTime);
            StartCoroutine(FallRoutine());
        }
        else
        {
            currentState = State.Idle;
        }
    }
}