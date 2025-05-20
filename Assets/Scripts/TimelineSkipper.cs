using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TimelineSkipper : MonoBehaviour
{
    public VideoPlayer videoPlayer;             // Asigna tu VideoPlayer desde el inspector
    public string sceneToLoadAfterSkip = "";    // Nombre de la escena (opcional)

    public void SkipVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.gameObject.SetActive(false); // Opcional: oculta el objeto del video
        }

        if (!string.IsNullOrEmpty(sceneToLoadAfterSkip))
        {
            SceneManager.LoadScene(sceneToLoadAfterSkip);
        }
    }
}
