using UnityEngine;
using UnityEngine.UI;

public class SecurityPanel : MonoBehaviour
{
    public Image[] mainButtons; // Array of the main button images
    public Image[] linkedIndicators; // Array of indicator images that will change between green and red
    public Sprite selectedSprite; // The sprite to indicate the selected button
    public Sprite defaultSprite; // The sprite for buttons when they are not selected
    public Sprite greenSprite; // Sprite for the green state
    public Sprite redSprite; // Sprite for the red state

    private int currentButtonIndex = 0; // Index of the currently selected button
    private bool[] isGreenStates; // Array to store the green/red state of each linked indicator

    private void Start()
    {
        isGreenStates = new bool[linkedIndicators.Length];
        LoadProgress(); // Load saved progress
        UpdateButtonSprites();
    }

    private void Update()
    {
        // Use WSAD keys for navigation
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentButtonIndex = (currentButtonIndex + 1) % mainButtons.Length;
            UpdateButtonSprites();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            currentButtonIndex = (currentButtonIndex - 1 + mainButtons.Length) % mainButtons.Length;
            UpdateButtonSprites();
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            // Toggle the linked indicator between green and red
            isGreenStates[currentButtonIndex] = !isGreenStates[currentButtonIndex];
            linkedIndicators[currentButtonIndex].sprite = isGreenStates[currentButtonIndex] ? greenSprite : redSprite;
            SaveProgress(); // Save progress
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // Deactivate the panel and resume the game
            gameObject.SetActive(false); // Deactivate the panel
            Time.timeScale = 1f; // Resume the game
        }
    }

    // Updates the sprites of the main buttons based on the current selection
    private void UpdateButtonSprites()
    {
        for (int i = 0; i < mainButtons.Length; i++)
        {
            if (i == currentButtonIndex)
            {
                mainButtons[i].sprite = selectedSprite; // Highlight selected button
            }
            else
            {
                mainButtons[i].sprite = defaultSprite; // Use default sprite for unselected buttons
            }

            linkedIndicators[i].sprite = isGreenStates[i] ? greenSprite : redSprite;
        }
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt("CurrentButtonIndex", currentButtonIndex);
        for (int i = 0; i < isGreenStates.Length; i++)
        {
            PlayerPrefs.SetInt("IsGreenState" + i, isGreenStates[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        currentButtonIndex = PlayerPrefs.GetInt("CurrentButtonIndex", 0);
        for (int i = 0; i < isGreenStates.Length; i++)
        {
            isGreenStates[i] = PlayerPrefs.GetInt("IsGreenState" + i, 1) == 1;
        }
    }
}
