using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in Inspector
    public string nextSceneName;    // Name of the scene to load after video

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd; // Listen for video end
        videoPlayer.Play();                        // Optional if not set to play on awake
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName); // Load the next scene when video ends
    }
}
