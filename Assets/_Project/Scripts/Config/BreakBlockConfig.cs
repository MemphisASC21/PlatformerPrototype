using UnityEngine;

namespace Platformer.Config
{
    [CreateAssetMenu(menuName = "Platformer/Hazards/Block Config")]
    public class BreakBlockConfig : ScriptableObject
    {
        public float breakDelay = 0.5f;
    }
}