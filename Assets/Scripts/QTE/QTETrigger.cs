using UnityEngine;
using UnityEngine.SceneManagement;

public class QTETrigger : MonoBehaviour
{
    public string quickTimeSceneName = "QTEScene"; // The scene where the QTE will happen

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Create a temporary GameObject to hold the QuickTimeManager script
            GameObject qteManagerObject = new GameObject("QuickTimeManager");
            QuickTimeManager qteManager = qteManagerObject.AddComponent<QuickTimeManager>();

            // Set the QTE scene name
            qteManager.quickTimeSceneName = quickTimeSceneName;

            // Set this object to not be destroyed when changing scenes
            DontDestroyOnLoad(qteManagerObject);

            // Load the QTE scene additively
            SceneManager.LoadScene(quickTimeSceneName, LoadSceneMode.Additive);
        }
    }
}
