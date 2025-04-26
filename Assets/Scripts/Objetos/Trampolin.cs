using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolin : MonoBehaviour
{
    public float bounceForce = 15f; // Force of the bounce
    public LayerMask playerLayer;   // Assign "Player" layer here for better filtering

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collided object is in the player layer
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            // Check if the player is hitting from above
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y <= -0.5f) // Coming down onto the trampoline
                {
                    Rigidbody2D playerRb = collision.rigidbody;
                    if (playerRb != null)
                    {
                        // Zero out existing Y velocity to make the bounce clean
                        Vector2 velocity = playerRb.velocity;
                        velocity.y = 0f;
                        playerRb.velocity = velocity;

                        // Apply upward force
                        playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                        Debug.Log("Player bounced!");
                    }
                    break; // We only need to bounce once
                }
            }
        }
    }
}
