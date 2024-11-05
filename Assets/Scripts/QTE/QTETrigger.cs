using UnityEngine;
using UnityEngine.SceneManagement;

public class QTETrigger : MonoBehaviour
{
    public string quickTimeSceneName = "QTEScene"; // The scene where the QTE will happen
    public string returnSceneName; // The scene to return to after the QTE ends

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Create a temporary GameObject to hold the QuickTimeManager script
            GameObject qteManagerObject = new GameObject("QuickTimeManager");
            QuickTimeManager qteManager = qteManagerObject.AddComponent<QuickTimeManager>();

            // Set the QTE scene name and return scene name
            qteManager.quickTimeSceneName = quickTimeSceneName;
            qteManager.returnSceneName = returnSceneName;

            // Set this object to not be destroyed when changing scenes
            DontDestroyOnLoad(qteManagerObject);

            // Load the QTE scene additively
            SceneManager.LoadScene(quickTimeSceneName, LoadSceneMode.Additive);
        }
    }
}
