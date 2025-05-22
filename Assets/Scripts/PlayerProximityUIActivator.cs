using UnityEngine;
using UnityEngine.UI;

public class PlayerProximityUIActivator : MonoBehaviour
{
    public Collider2D triggerCollider;
    public LayerMask playerLayer;
    public GameObject uiImage;

    private bool isPlayerInside = false;
    private bool hasBeenShown = false; // ✅ Nuevo control

    private void Update()
    {
        CheckForPlayerProximity();
    }

    private void CheckForPlayerProximity()
    {
        Collider2D hit = Physics2D.OverlapBox(triggerCollider.bounds.center, triggerCollider.bounds.size, 0f, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            if (!isPlayerInside && !hasBeenShown)
            {
                isPlayerInside = true;
                hasBeenShown = true;            // ✅ Evita que se vuelva a mostrar
                uiImage.SetActive(true);
                Debug.Log("Player entered trigger zone");
            }
        }
        else
        {
            if (isPlayerInside)
            {
                isPlayerInside = false;
                uiImage.SetActive(false);
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
