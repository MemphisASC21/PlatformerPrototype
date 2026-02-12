using Platformer.Player;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Platformer.Config;

public class ThwompBehavior : MonoBehaviour
{
    /*------------------------------ABOUT THE SCRIPT------------------------
     * This is for the falling blocks. What it does is fall and rise at variable times (set in inspector). We can make fast
     * and slow falling blocks this way. It sends a raycast down to detetct the ground to determine how far it "falls". 
     * 
     * Make sure you place thwomps NOT touching celings. If the box raycast is touching the celing (tagged as "ground") it will fire
     * and "hit" the celing and won't move down.
     */

    private enum State { Idle, Falling, Rising, Cooldown }
    private State currentState = State.Idle;
    //--FOR HEALTH SCRIPT, look at how vamp survivors did it-- private PlayerHealth _playerHealth;

    [Header("Thwomp Settings")]
    [SerializeField] private ThwompConfig config;

    private Vector3 startPos;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private bool isPlayerUnder = false;

    /*box cast params
    BoxCollider collider = (BoxCollider)gameObject.GetComponent<Collider>();
    private float yHalfExtents = collider.bounds.extents.y;
    private float yCenter = collider.bounds.center.y;
    private float yUpper = transform.position.y + (yCenter + yHalfExtents);
    private float yLower = transform.position.y + (yCenter - yHalfExtents);
    */

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
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

    /* ------Health----------
     * also, make sure that the health is ONLY subtracted when the state is falling. That way, if the player bumps into it while rising
     * they don't die, same with cooldown (ex. they jump and hit the bottom of it.)
     */
     private void OnCollisionEnter2D(Collision2D collision)
     {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth) && currentState==State.Falling)
        {
            //Debug.Log("Player collided with thwomp");
            playerHealth.TakeDamage(config.damage);
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
        Vector2 boxSize = new Vector2(col.bounds.size.x * 0.95f, col.bounds.size.y);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, Mathf.Infinity, config.groundLayer);

        if (hit.collider == null)
        {
            currentState = State.Idle;
            yield break; 
        }

        float distanceToGround = hit.distance;
        float targetY = transform.position.y - distanceToGround;
        
        //fall
        while (transform.position.y > targetY)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, 0), config.fallSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, targetY, 0);
        currentState = State.Cooldown;
        yield return new WaitForSeconds(config.bottomWaitTime);

        currentState = State.Rising;

        while (Vector2.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, config.riseSpeed * Time.deltaTime);
            yield return null;
        }
        
        transform.position = startPos;

        if (isPlayerUnder)
        {
            currentState = State.Cooldown;
            yield return new WaitForSeconds(config.topWaitTime);
            StartCoroutine(FallRoutine());
        }
        else
        {
            currentState = State.Idle;
        }
    }
}