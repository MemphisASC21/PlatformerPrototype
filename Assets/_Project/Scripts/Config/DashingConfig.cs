using UnityEngine;

namespace Platformer.Config
{
    [CreateAssetMenu(fileName = "DashConfig", menuName = "Platformer/Dash Config")]
    public class DashConfig : ScriptableObject
    {
        [Header("Dash")]
        [Tooltip("How fast the dash moves.")]
        [Range(10f, 40f)]
        public float dashSpeed = 290f;

        [Tooltip("How long the dash lasts in seconds.")]
        [Range(0.05f, 0.3f)]
        public float dashDuration = 0.15f;

        [Tooltip("Cooldown before you can dash again.")]
        [Range(0f, 2f)]
        public float dashCooldown = 0.5f;

        [Tooltip("If true, dash refreshes when you land.")]
        public bool resetOnGround = true;
    }
}