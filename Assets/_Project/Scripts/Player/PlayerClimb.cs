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

    [Header("Configuration")]
    [SerializeField] private ClimbConfig climbConfig; 
    [SerializeField] private MovementConfig gravityConfig;

    //professor: it was bugging when I set it up in config. It would spawn then get left behind or not show up at all :(
    [Header("Effects")]
    [SerializeField] private ParticleSystem climbVfx;

    private Rigidbody2D rb;
    private InputReader inputReader;
    private Coroutine activeClimbRoutine;
    //this needs to be accessed by dash climb
    public bool isClimbing { get; private set; }

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
        float currentGripTimer = climbConfig.maxGripTime;

        /*---Reset variables for climbing physics---*/
        //Turn off gravity. Since we have the slide down wall THEN fall, we have to turn off gravity so they don't just fall.
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        if (climbVfx != null) climbVfx.Play();


        // Loop forever until the Coroutine is stopped by OnTriggerExit or Jump
        while (true)
        {
            if (inputReader == null) yield break;

            //Memphis's dash logic
            if (inputReader.DashBuffered)
            {
                inputReader.ConsumeDashBuffer();
                float dashTimer = climbConfig.dashClimbDuration;

                while (dashTimer > 0)
                {
                    rb.linearVelocity = new Vector2(0f, climbConfig.dashClimbSpeed);
                    dashTimer -= Time.deltaTime;
                    yield return null;
                }
                rb.linearVelocity = Vector2.zero;
            }

            if (inputReader.JumpBuffered)
            {
                inputReader.ConsumeJumpBuffer();
                PerformWallJump();
                yield break;
            }

            float yInput = inputReader.MoveInput.y;
            if (yInput > 0.1f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbConfig.climbSpeed);
                currentGripTimer = climbConfig.maxGripTime;
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -climbConfig.slideSpeed);
                currentGripTimer -= Time.deltaTime;

                if (currentGripTimer <= 0)
                {
                    if (climbVfx != null) climbVfx.Stop();
                    rb.gravityScale = gravityConfig.gravityScale;
                    activeClimbRoutine = null;
                    isClimbing = false;
                    yield break;
                }
            }

            yield return null;
        }
    }

        /*----------Previous climb logic, before climb + dash implimentation-----------------
         * 
         * if (inputReader == null) yield break;
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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbConfig.climbSpeed);
            currentGripTimer = climbConfig.maxGripTime;
        }
        else //if sliding Down
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -climbConfig.slideSpeed);
            currentGripTimer -= Time.deltaTime;


            if (currentGripTimer <= 0)
            {
                //grip lost, turn gravity back on and kill this routine
                rb.gravityScale = gravityConfig.gravityScale;
                activeClimbRoutine = null;
                isClimbing = false;
                yield break;
            }
        }
        yield return null;*/

    private void PerformWallJump()
    {
        float jumpDir = -Mathf.Sign(transform.localScale.x);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(jumpDir * climbConfig.wallJumpForce.x, climbConfig.wallJumpForce.y), ForceMode2D.Impulse);

        if (climbVfx != null) climbVfx.Stop();

        rb.gravityScale = gravityConfig.gravityScale;
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
            if (climbVfx != null) climbVfx.Stop();
            isClimbing = false;
            rb.gravityScale = gravityConfig.gravityScale;
        }
    }
}
