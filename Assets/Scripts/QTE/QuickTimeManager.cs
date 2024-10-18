using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickTimeManager : MonoBehaviour
{
    public string quickTimeSceneName = "QTEScene";
    public PointsManager pointsManager; // Reference to the PointsManager
    public Text feedbackText; // UI Text element to show feedback

    private string originalSceneName;
    private bool eventCompleted = false;

    private void Start()
    {
        // Get the original scene name to return to
        originalSceneName = SceneManager.GetActiveScene().name;

        // Hide the feedback text initially
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Only check for input if the event hasn't been completed yet
        if (!eventCompleted)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Add heart points if "Q" is pressed
                pointsManager.AddHeartPoints(1);
                ShowFeedback("El camino del corazon!");
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                // Add mind points if "E" is pressed
                pointsManager.AddMindPoints(1);
                ShowFeedback("El camino de la mente!");
            }
        }
    }

    // Displays the feedback and returns to the original scene after a delay
    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.gameObject.SetActive(true);
        }

        eventCompleted = true;
        Invoke("ReturnToOriginalScene", 2f); // Return after 2 seconds
    }

    private void ReturnToOriginalScene()
    {
        // Hide the feedback text before transitioning back
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }

        // Unload the QTE scene, keeping the original scene active
        SceneManager.UnloadSceneAsync(quickTimeSceneName);

        // Destroy the QuickTimeManager after returning to clean up
        Destroy(gameObject);
    }
}
