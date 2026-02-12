using Platformer.Config;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableBlockBehavior : MonoBehaviour
{
    [SerializeField] private BreakBlockConfig config;

    void Start()
    {

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
        //vfx logic below if we get to it

        //replaced the vfx check with null check in the meantime
        if (config != null && config.breakDelay > 0)
        {
            yield return new WaitForSeconds(config.breakDelay);
        }
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }
}
