using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickTimeManager : MonoBehaviour
{
    public string quickTimeSceneName = "QTEScene";
    public string returnSceneName; // The scene to return to after the QTE ends
    public PointsManager pointsManager; // Reference to the PointsManager
    public Text feedbackText; // UI Text element to show feedback

    private bool eventCompleted = false;

    public static bool dashUnlocked = false;
    public static bool doubleJumpUnlocked = false;


    private void Start()
    {
        // Hide the feedback text initially
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }

        // Fix for multiple Event Systems issue
        DestroyExtraEventSystems();

        // Fix for multiple Audio Listeners issue
        DisableExtraAudioListeners();
    }

    private void Update()
    {
        // Only check for input if the event hasn't been completed yet
        if (!eventCompleted)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                pointsManager.AddHeartPoints(1);
                dashUnlocked = true; // ✅ Unlock dash
                ShowFeedback("¡El camino del corazón! Dash desbloqueado.");
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                pointsManager.AddMindPoints(1);
                doubleJumpUnlocked = true; // ✅ Unlock double jump
                ShowFeedback("¡El camino de la mente! Doble salto desbloqueado.");
            }

        }
    }

    // Displays the feedback and returns to the specified scene after a delay
    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.gameObject.SetActive(true);
        }

        eventCompleted = true;
        Invoke("ReturnToSpecifiedScene", 2f); // Return after 2 seconds
    }

    private void ReturnToSpecifiedScene()
    {
        // Hide the feedback text before transitioning back
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }

        // Unload the QTE scene
        SceneManager.UnloadSceneAsync(quickTimeSceneName);

        // Load the specified return scene
        SceneManager.LoadScene(returnSceneName);

        // Destroy the QuickTimeManager to clean up
        Destroy(gameObject);
    }

    // Method to destroy extra Event Systems to fix the "Only one active Event System" issue
    private void DestroyExtraEventSystems()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);
            }
        }
    }

    // Method to disable extra Audio Listeners to fix the "2 audio listeners" issue
    private void DisableExtraAudioListeners()
    {
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();
        if (audioListeners.Length > 1)
        {
            for (int i = 1; i < audioListeners.Length; i++)
            {
                audioListeners[i].enabled = false;
            }
        }
    }
}
