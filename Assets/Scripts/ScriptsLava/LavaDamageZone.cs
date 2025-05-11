using UnityEngine;

public class LavaDamageZone : MonoBehaviour
{
    public LavaController lavaController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lavaController.HandlePlayerHit(other.gameObject);
        }
    }
}
