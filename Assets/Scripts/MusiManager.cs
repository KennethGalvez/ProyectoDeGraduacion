using UnityEngine;
using UnityEngine.SceneManagement; 

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evita que el objeto se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void OnEnable()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
        if (scene.name == "Nivel 1")
        {

            GetComponent<AudioSource>().Stop();
        }
    }
}
