using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Assign your Pause Panel here
    private bool isPaused = false;
    private AudioSource[] allAudioSources;

    void Start()
    {
        pausePanel.SetActive(false);
        EnsurePausePanelIsOnTop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        // Show pause panel and stop time
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Pause all AudioSources
        allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Pause();
        }

        // Ensure the panel is on top
        EnsurePausePanelIsOnTop();
    }

    public void ResumeGame()
    {
        // Resume time and hide panel
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;

        // Resume all AudioSources
        if (allAudioSources != null)
        {
            foreach (AudioSource audio in allAudioSources)
            {
                audio.UnPause();
            }
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void EnsurePausePanelIsOnTop()
    {
        Canvas parentCanvas = pausePanel.GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            parentCanvas.sortingOrder = 9999; // Push to the top visually
        }
    }
}
