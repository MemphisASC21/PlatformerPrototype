using UnityEngine;
using Platformer.Core; 
using Platformer.Player;
using System.Collections;
using Platformer.Config;

public class PlayerClimb : MonoBehaviour
{
    /*-------------LOGIC FOR WALL CLIMB, FEEL FREE TO CHECK IF LOGIC IS ACCURATE---------------
     * Trigger colliders on areas where they can climb. Should we have ladders so it
     * is easier for the player to see specifically where to climb? Or does that make it too walk-through-y?
     * --The goal here is to build a ducklife ish climb mechanic--
     * Instead of states, If touch wall, facing foward, & holding up input > Move up (state: climb) (disable gravity? translate rb up)
     * If touch wall, isClimbing, NOT holding up > slide down slowly (state: slide)
     * If isSliding for 2s > let go and fall off wall.
     * If isClimbing and jump input > launch 45 degrees up and away from direction facing.
     */

    /*---------SETUP IN EDITOR--------
     * Create walls as normal. Make sure if the player is standing on top if it, it is on the "ground"
     * layer. If not, the player won't be able to run ground check and can't jump. Once you have the walls
     * in place, create an empty "ladder" with a box collider and check isTrigger. Tag this null as
     * climbable. Make sure the collider doesn't extend above the wall where the player can walk, or too
     * far out. Later we can add a ladder sprite if we want climbable areas to be visible
     */

    /*----------HEALTH--------------
     * Right now, the script doesn't have the player lose health if they fall from high height. If
     * we want to do this, we can talk about it in class.
     */

    [Header("Settings")]
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float slideSpeed = 2f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(10f, 15f);
    [SerializeField] private float maxGripTime = 2f;

    //this is how we get our gravity back
    [SerializeField] private MovementConfig config;

    private Rigidbody2D rb;
    private InputReader inputReader;
    private Coroutine activeClimbRoutine;
    private bool isClimbing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //This is to find the input reader so we can see jump mechanics, and later impliment dash mechanics
        if (ServiceLocator.Has<InputReader>())
        {
            inputReader = ServiceLocator.Get<InputReader>();
        }
        else
        {
            Debug.LogError("InputReader not found.");
        }
    }
    /*When player enters the tigger for walls, we start the coroutine for climbing. This coroutine checks
     * for the climbing logic that way we don't have to call the logic on update
     */
    private void OnTriggerStay2D(Collider2D other)
    {
        //must be at climbable wall, must be not already climbing state, and must be pressing up.
        if (other.CompareTag("Climbable") && !isClimbing)
        {
            // If holding UP ... OR ... pushing TOWARDS the wall (Input X matches Wall Direction)
            float directionToWall = other.transform.position.x - transform.position.x;
            bool pushingAgainstWall = Mathf.Sign(inputReader.MoveInput.x) == Mathf.Sign(directionToWall) && Mathf.Abs(inputReader.MoveInput.x) > 0.1f;

            if (inputReader.MoveInput.y > 0.1f || pushingAgainstWall)
            {
                activeClimbRoutine = StartCoroutine(ClimbRoutine());
            }
        }
    }

    /*This is like the equivalent of calling Update but it only does it while the player is climbing.
     * That way we don't have unneccessary calls on update.
     */
    private IEnumerator ClimbRoutine()
    {
        isClimbing = true;
        float currentGripTimer = maxGripTime;
        
        /*---Reset variables for climbing physics---*/
        //Turn off gravity. Since we have the slide down wall THEN fall, we have to turn off gravity so they don't just fall.
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        // Loop forever until the Coroutine is stopped by OnTriggerExit or Jump
        while (true)
        {
            // Safety Check
            if (inputReader == null) yield break;
            float yInput = inputReader.MoveInput.y;

            //wall jump
            if (inputReader.JumpBuffered)
            {
                inputReader.ConsumeJumpBuffer();
                PerformWallJump();
                yield break;
            }

            if (yInput > 0.1f) //check later, this may bug due to jumping. 
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbSpeed);
                currentGripTimer = maxGripTime; //refresh timer
            }
            else //if sliding Down
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -slideSpeed);
                currentGripTimer -= Time.deltaTime; //decreases the gri time, so after our grip ability they fall off 


                if (currentGripTimer <= 0)
                {
                    //grip lost, turn gravity back on and kill this routine
                    rb.gravityScale = config.gravityScale;
                    activeClimbRoutine = null;
                    isClimbing = false;
                    yield break;
                }
            }
            yield return null;
        }
    }

    private void PerformWallJump()
    {
        float jumpDir = -Mathf.Sign(transform.localScale.x);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(jumpDir * wallJumpForce.x, wallJumpForce.y), ForceMode2D.Impulse);

        rb.gravityScale = config.gravityScale;
        activeClimbRoutine = null;
        isClimbing = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Climbable") && isClimbing)
        {
            if (activeClimbRoutine != null)
            {
                StopCoroutine(activeClimbRoutine);
                activeClimbRoutine = null;
            }
            isClimbing = false;
            rb.gravityScale = config.gravityScale;
        }
    }
}
