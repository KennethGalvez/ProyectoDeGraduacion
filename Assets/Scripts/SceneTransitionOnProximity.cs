using UnityEngine;

public class SceneTransitionOnProximity : MonoBehaviour
{
    public GameObject securityPanel;
    public float interactionDistance = 2f; 
    public Transform player; 
    public SecurityPanel securityPanelScript; // SecurityPanel script

    private bool isPanelActive = false;

    private void Update()
    {
        // Check if the player is close enough and the panel is not permanently unlocked
        if (Vector3.Distance(transform.position, player.position) <= interactionDistance && !securityPanelScript.IsUnlocked)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isPanelActive)
            {
                // Activate the panel and freeze the game
                Time.timeScale = 0f; // Freeze the game
                securityPanel.SetActive(true); // Activate the panel
                isPanelActive = true;
            }
        }
    }

    public void ClosePanel()
    {
        // Close the panel and unfreeze the game
        securityPanel.SetActive(false);
        Time.timeScale = 1f;
        isPanelActive = false;
    }
}
