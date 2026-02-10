using UnityEngine;
using Platformer.Config;
using Platformer.Core;

namespace Platformer.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DashController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private TrailRenderer dashTrail;
        [SerializeField] private DashConfig config;

        // Public state — other scripts can read these but not change them
        public bool IsDashing { get; private set; }
        public bool CanDash => !IsDashing && cooldownTimer <= 0f && hasDash;

        // References
        private Rigidbody2D rb;
        private InputReader inputReader;
        private PlayerController playerController;

        // Internal state
        private float dashTimer;
        private float cooldownTimer;
        private float dashDirection;
        private bool hasDash = true;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            inputReader = ServiceLocator.Get<InputReader>();
            playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            // Tick cooldown
            if (cooldownTimer > 0f)
                cooldownTimer -= Time.deltaTime;

            // Check for buffered dash input
            if (inputReader.DashBuffered && CanDash)
            {
                StartDash();
                inputReader.ConsumeDashBuffer();
            }
        }

        private void FixedUpdate()
        {
            // Reset dash when grounded (if configured)
            if (config.resetOnGround && playerController.IsGrounded)
                hasDash = true;

            // Run dash physics
            if (IsDashing)
            {
                dashTimer -= Time.fixedDeltaTime;

                if (dashTimer <= 0f)
                {
                    EndDash();
                }
                else
                {
                    // Maintain dash velocity
                    rb.linearVelocity = new Vector2(dashDirection * config.dashSpeed, 0f);
                }
            }
        }

        private void StartDash()
        {
            IsDashing = true;
            hasDash = false;
            dashTimer = config.dashDuration;
            dashTrail.emitting = true;

            // Dash in input direction, or facing direction if no input
            float inputX = inputReader.MoveInput.x;
            if (Mathf.Abs(inputX) > 0.1f)
                dashDirection = Mathf.Sign(inputX);
            else
                dashDirection = rb.linearVelocity.x >= 0 ? 1f : -1f;

            // Set dash velocity and disable gravity
            rb.linearVelocity = new Vector2(dashDirection * config.dashSpeed, 0f);
            rb.gravityScale = 0f;
        }

        private void EndDash()
        {
            IsDashing = false;
            cooldownTimer = config.dashCooldown;
            dashTrail.emitting = false;

            // Restore gravity and reduce exit speed
            rb.gravityScale = 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, rb.linearVelocity.y);
        }
    }
}
