using UnityEngine;

public class BobbingBehavior : MonoBehaviour
{
    /*----------ABOUT--------
     * This is just a basic script to make things "bob" up and down. It is applied to our checkpoints prefab but
     * we can add it to pickups if we add them.
     */

    [Header("Floating Settings")]
    [SerializeField] private float floatSpeed = 2f; 
    [SerializeField] private float floatHeight = 0.5f;

    private Vector3 startPos;
    private float randomOffset;

    void Start()
    {
    startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin((Time.time * floatSpeed)) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
