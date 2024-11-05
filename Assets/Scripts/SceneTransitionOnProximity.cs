using UnityEngine;

public class SceneTransitionOnProximity : MonoBehaviour
{
    public GameObject securityPanel; // Reference to the security panel GameObject
    public float interactionDistance = 2f; // Distance within which the player can interact
    public Transform player; // Reference to the player object

    private bool isPanelActive = false;

    private void Update()
    {
        // Check the distance between the player and this object
        if (Vector3.Distance(transform.position, player.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isPanelActive)
            {
                // Freeze the game and activate the security panel
                Time.timeScale = 0f; // Freeze the game
                securityPanel.SetActive(true); // Activate the panel
                isPanelActive = true;
            }
        }
    }
}
