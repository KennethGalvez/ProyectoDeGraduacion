using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool doorClosed = true;

    private void Update()
    {
        if (doorClosed && SecurityPanel.DoorShouldOpen)
        {
            gameObject.SetActive(false); // Deactivate the door
            doorClosed = false; // Make sure we don't check again unnecessarily
            Debug.Log("Door deactivated because the panel was solved!");
        }
    }
}
