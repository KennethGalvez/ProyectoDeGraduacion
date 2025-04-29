using UnityEngine;
using UnityEngine.UI;

public class PlayerProximityUIActivator : MonoBehaviour
{
    public Collider2D triggerCollider;       // Assign the BoxCollider2D attached to this object
    public LayerMask playerLayer;            // Assign to "Player" layer
    public GameObject uiImage;               // Assign the Image (or any UI GameObject) in your Canva

    private bool isPlayerInside = false;

    private void Update()
    {
        CheckForPlayerProximity();
    }

    private void CheckForPlayerProximity()
    {
        Collider2D hit = Physics2D.OverlapBox(triggerCollider.bounds.center, triggerCollider.bounds.size, 0f, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            if (!isPlayerInside)
            {
                isPlayerInside = true;
                uiImage.SetActive(true); // Show image
                Debug.Log("Player entered trigger zone");
            }
        }
        else
        {
            if (isPlayerInside)
            {
                isPlayerInside = false;
                uiImage.SetActive(false); // Hide image
                Debug.Log("Player left trigger zone");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (triggerCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(triggerCollider.bounds.center, triggerCollider.bounds.size);
        }
    }
}
