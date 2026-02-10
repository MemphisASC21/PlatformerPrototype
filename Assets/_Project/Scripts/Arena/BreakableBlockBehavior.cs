using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableBlockBehavior : MonoBehaviour
{
    //how to get the block object
    public GameObject breakVFX;
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(BlockBreak());
        }
    }
    IEnumerator BlockBreak()
    {
        //vfx logic
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }
}
