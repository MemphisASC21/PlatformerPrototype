using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //checkpoint number
    [Header("Checkpoint Number Set")]
    [SerializeField] public int checkpointNum;

    private Vector3 checkpointPosition;
    public Transform spriteChildTransform;

    public GameManager gameManager;
    //when player enters checkpoint it runs everything

    void Start()
    {
        checkpointPosition = spriteChildTransform.position;
    }

    /*void Update()
    {
        this.Up.transform.position * Time.deltaTime;
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.UpdateCheckpoint(checkpointNum, checkpointPosition);
    }
}
