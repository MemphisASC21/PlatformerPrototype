using UnityEngine;

[CreateAssetMenu(fileName = "ClimbConfig", menuName = "Platformer/Climb Config")]
public class ClimbConfig : ScriptableObject
{
    [Header("Climb Settings")]
    public float climbSpeed = 5f;
    public float slideSpeed = 2f;
    public float dashClimbMultiplier = 2.0f;
    public Vector2 wallJumpForce = new Vector2(10f, 15f);
    public float maxGripTime = 2f;

    [Header("Climb Dash")]
    public float dashClimbSpeed = 20f;   // Very fast upward burst
    public float dashClimbDuration = 0.2f; // Short duration
}
