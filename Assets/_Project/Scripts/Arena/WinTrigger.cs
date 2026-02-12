using Platformer.Player;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //stop player, this is also in playerhealth for when they die. is it redundant ?maybe when any "game over UI" is
            //shown, we call these commands. this freezes the player in place though, so if they win, they win stuck in whatever
            //pose they entered the trigger with.

            var controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.enabled = false;
            }

            var rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            FindFirstObjectByType<UIPanel>().ShowWinScreen();
            //vfx for winning here if we want
        }
    }
}
