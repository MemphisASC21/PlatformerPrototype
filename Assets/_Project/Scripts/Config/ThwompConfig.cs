using UnityEngine;

namespace Platformer.Config
{
    [CreateAssetMenu(menuName = "Platformer/Hazards/Thwomp Config")]
    public class ThwompConfig : ScriptableObject
    {
        public float damage = 15f;
        public float fallSpeed = 12f;
        public float riseSpeed = 5f;
        public float bottomWaitTime = 1f;
        public float topWaitTime = 0.5f;
        public LayerMask groundLayer;
    }
}
