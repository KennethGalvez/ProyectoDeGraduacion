using UnityEngine;
using UnityEngine.SceneManagement;

public class QTETrigger : MonoBehaviour
{
    public string quickTimeSceneName = "QTEScene"; // Escena donde ocurre el QTE

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(quickTimeSceneName); // Cambia completamente a la escena QTE
        }
    }
}
