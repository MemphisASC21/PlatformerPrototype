using UnityEngine;

[CreateAssetMenu(menuName = "Platformer/Camera Config")]
public class CameraConfig : ScriptableObject
{
    [Header("Landing Shake")]
    public float landDuration = 0.1f;
    public float landMagnitude = 0.05f;

    [Header("Damage Shake")]
    public float damageDuration = 0.2f;
    public float damageMagnitude = 0.3f;

    [Header("Thwomp Impact Shake")]
    public float thwompHitDuration = 0.3f;
    public float thwompHitMagnitude = 0.6f;
}
