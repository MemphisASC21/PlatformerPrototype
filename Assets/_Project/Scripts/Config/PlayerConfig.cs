using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Platformer/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    /*----------ABOUT--------
     * While it is an emplty config, I am setting this up so in the future if we have more player
     * variables, they can be refrenced here.
     */

    [Header("Health Settings")]
    public float maxHealth = 100f;
}
