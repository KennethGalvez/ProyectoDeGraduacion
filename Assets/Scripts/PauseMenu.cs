using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject heartGroup;
    public GameObject mindGroup;

    [Header("Ability Progress Bar")]
    public Image abilityBar;                 // Barra de progreso (Filled)
    public Color heartColor = Color.red;     // Color para el camino del corazón
    public Color mindColor = Color.blue;     // Color para el camino de la mente

    private bool isPaused = false;
    private AudioSource[] allAudioSources;

    private Dictionary<GameObject, bool> originalUIStates = new Dictionary<GameObject, bool>();

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
        pausePanel.SetActive(true);
        UpdateAbilityGroups();
        CacheAndHideOtherUI();
        Time.timeScale = 0f;
        isPaused = true;

        allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Pause();
        }

        EnsurePausePanelIsOnTop();
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        RestoreUIStates();
        Time.timeScale = 1f;
        isPaused = false;

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
            parentCanvas.sortingOrder = 9999;
        }
    }

    private void UpdateAbilityGroups()
    {
        if (QuickTimeManager.dashUnlocked)
        {
            heartGroup.SetActive(true);
            mindGroup.SetActive(false);
            UpdateAbilityBar(1f, heartColor);
        }
        else if (QuickTimeManager.doubleJumpUnlocked)
        {
            heartGroup.SetActive(false);
            mindGroup.SetActive(true);
            UpdateAbilityBar(1f, mindColor);
        }
        else
        {
            heartGroup.SetActive(false);
            mindGroup.SetActive(false);
            UpdateAbilityBar(0f, Color.clear);
        }
    }

    private void UpdateAbilityBar(float fillAmount, Color fillColor)
    {
        if (abilityBar != null)
        {
            abilityBar.fillAmount = fillAmount;
            abilityBar.color = fillColor;
        }
    }

    private void CacheAndHideOtherUI()
    {
        originalUIStates.Clear();

        Canvas parentCanvas = pausePanel.GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            foreach (Transform child in parentCanvas.transform)
            {
                if (child.gameObject != pausePanel)
                {
                    originalUIStates[child.gameObject] = child.gameObject.activeSelf;
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    private void RestoreUIStates()
    {
        foreach (var kvp in originalUIStates)
        {
            kvp.Key.SetActive(kvp.Value);
        }
        originalUIStates.Clear();
    }
}
