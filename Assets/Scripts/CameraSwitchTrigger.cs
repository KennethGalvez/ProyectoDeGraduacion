using UnityEngine;
using Cinemachine;

public class CameraSwitchTrigger : MonoBehaviour
{
    public Collider2D triggerCollider;
    public LayerMask playerLayer;

    public CinemachineVirtualCamera currentCamera;
    public CinemachineVirtualCamera newCamera;

    [Header("Optional GameObject to Activate")]
    public GameObject objectToActivate; // ✅ Objeto que se activará al cruzar

    private bool hasSwitched = false;

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

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log("Activated: " + objectToActivate.name);
        }
    }

    private void OnDrawGizmos()
    {
        if (triggerCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(triggerCollider.bounds.center, triggerCollider.bounds.size);
        }
    }
}
