using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Transform teleportPoint; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit the spikes! Teleporting...");
            TeleportCharacter(other.gameObject);
        }
    }

    
    private void TeleportCharacter(GameObject character)
    {
        if (teleportPoint != null)
        {
            character.transform.position = teleportPoint.position;
            Debug.Log("Player teleported to: " + teleportPoint.position);
        }
        else
        {
            Debug.LogWarning("Teleport point not set for the SpikeTrap!");
        }
    }
}
