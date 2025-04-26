using UnityEngine;
using Cinemachine; // Make sure you have using Cinemachine!

public class CameraSwitchTrigger : MonoBehaviour
{
    public Collider2D triggerCollider; // Collider attached to the empty GameObject
    public LayerMask playerLayer; // Set this to the Player layer in Inspector

    public CinemachineVirtualCamera currentCamera; // Camera to deactivate
    public CinemachineVirtualCamera newCamera; // Camera to activate

    private bool hasSwitched = false; // Prevent multiple switches

    private void Update()
    {
        if (hasSwitched)
            return;

        CheckForPlayerEnter();
    }

    private void CheckForPlayerEnter()
    {
        Collider2D hit = Physics2D.OverlapBox(triggerCollider.bounds.center, triggerCollider.bounds.size, 0f, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            Debug.Log("Player entered camera switch zone!");
            SwitchCameras();
            hasSwitched = true;
        }
    }

    private void SwitchCameras()
    {
        if (currentCamera != null)
            currentCamera.gameObject.SetActive(false);

        if (newCamera != null)
            newCamera.gameObject.SetActive(true);
    }

    // Optional: draw the box in Scene view for easier setup
    private void OnDrawGizmos()
    {
        if (triggerCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(triggerCollider.bounds.center, triggerCollider.bounds.size);
        }
    }
}
