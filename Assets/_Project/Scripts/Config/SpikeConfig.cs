using UnityEngine;

namespace Platformer.Config
{
    [CreateAssetMenu(menuName = "Platformer/Hazards/Spike Config")]
    public class SpikeConfig : ScriptableObject
    {
        public float damage = 10f;
        public float attackSpeed = 15f;
        public float retractSpeed = 3f;
        public float spikeHeight = 1f;
        public float holdTime = 1f;
        public float cooldownTime = 0.5f;
    }
}