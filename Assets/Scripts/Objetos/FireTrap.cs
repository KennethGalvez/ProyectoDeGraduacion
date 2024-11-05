using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public Collider2D fireCollider; // Reference to the fire collider
    public Animator animator; // Reference to the Animator
    public float fireOnTime = 2f; // Time duration for the fire to be on
    public float fireOffTime = 2f; // Time duration for the fire to be off

    public Transform teleportPoint; 
    private bool isFireActive = false; 

    private void Start()
    {
        // fire cycle
        StartCoroutine(FireCycle());
    }

    // Coroutine  fire on and off intervals
    private System.Collections.IEnumerator FireCycle()
    {
        while (true)
        {
            // Activate the fire
            isFireActive = true;
            fireCollider.enabled = true;
            animator.SetBool("fuego", true); 
            Debug.Log("Fire is ON");

            // Wait for the fire on time
            yield return new WaitForSeconds(fireOnTime);

            // Deactivate the fire
            isFireActive = false;
            fireCollider.enabled = false;
            animator.SetBool("fuego", false); 
            Debug.Log("Fire is OFF");

            
            yield return new WaitForSeconds(fireOffTime);
        }
    }

    //Detectar Colision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFireActive && other.CompareTag("Player"))
        {
            Debug.Log("Player hit by fire!");
            TeleportCharacter(other.gameObject);
        }
    }

    // teleport
    private void TeleportCharacter(GameObject character)
    {
        if (teleportPoint != null)
        {
            character.transform.position = teleportPoint.position;
            Debug.Log("Player teleported to: " + teleportPoint.position);
        }
        else
        {
            Debug.LogWarning("Teleport point not set!");
        }
    }
}
