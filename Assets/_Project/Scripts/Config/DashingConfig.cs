using UnityEngine;

[CreateAssetMenu(fileName = "DashingConfig", menuName = "Scriptable Objects/DashingConfig")]
public class DashingConfig : ScriptableObject
{
    [Header("Dash Core")] // the basis of the dash logic
    [Min(0.1f)] public float dashSpeed = 18f;//so that the player does not enter negative numbers
    [Min(0.01f)] public float dashDuration = 0.15f; //so that the player does not enter negative numbers
    [Min(0f)] public float dashCooldown = 0.35f; //player has to wait a few seconds before hey can dash again

    [Header("Rules")] //parameters for the dash
    [Tooltip("If true, player can dash in the air. If false, player can only dash on the ground.")]
    public bool allowAirDash = true; // ground dash permitted if false
    [Min(0)] public int maxAirDashes = 1; //you can only dash x amount of times before touching the ground again

    [Header("Physics During Dash")]
    [Min(0f)] public float gravityScaleDuringDash = 0f; //no gravity when player is dashing
    public bool preserveVerticalVelocity = false; //if true, keep current y velocity

    [Header("Input Buffering")]
    [Min(0f)] public float dashBufferTime = 0.1f; //press dash slighly early
}
