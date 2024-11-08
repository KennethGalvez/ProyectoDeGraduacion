using UnityEngine;
using UnityEngine.UI;

public class SecurityPanel : MonoBehaviour
{
    public Image[] mainButtons; 
    public Image[] linkedIndicators; 
    public Sprite selectedSprite; 
    public Sprite defaultSprite; 
    public Sprite greenSprite; 
    public Sprite redSprite; 

    public bool[] correctPattern; 
    public AudioSource correctSound; 
    public AudioSource incorrectSound; 

    private int currentButtonIndex = 0; 
    private bool[] isGreenStates; 
    private bool isUnlocked = false; 

    public bool IsUnlocked => isUnlocked; 

    private void Start()
    {
        isGreenStates = new bool[linkedIndicators.Length];
        LoadProgress(); // Load saved progress
        UpdateButtonSprites();
    }

    private void Update()
    {
        if (isUnlocked)
            return; // Si el panel esta lockeado no hacer nada

        // WSAD para la navegacion
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
            // Toggle the linked indicator entre green and red
            isGreenStates[currentButtonIndex] = !isGreenStates[currentButtonIndex];
            linkedIndicators[currentButtonIndex].sprite = isGreenStates[currentButtonIndex] ? greenSprite : redSprite;
            SaveProgress(); // Save progress
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            
            FindObjectOfType<SceneTransitionOnProximity>().ClosePanel(); // SceneTransitionOnProximity script
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // Checkea el patron
            VerifyPattern();
        }
    }

    // Updates de sprites de los mains botones
    private void UpdateButtonSprites()
    {
        for (int i = 0; i < mainButtons.Length; i++)
        {
            if (i == currentButtonIndex)
            {
                mainButtons[i].sprite = selectedSprite; // selected button
            }
            else
            {
                mainButtons[i].sprite = defaultSprite; //  default sprite
            }

            linkedIndicators[i].sprite = isGreenStates[i] ? greenSprite : redSprite;
        }
    }

    // matchea el patron
    private void VerifyPattern()
    {
        bool isCorrect = true;

        for (int i = 0; i < isGreenStates.Length; i++)
        {
            if (isGreenStates[i] != correctPattern[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            // correcto sound
            correctSound.Play();
            isUnlocked = true; // unlock panel
            FindObjectOfType<SceneTransitionOnProximity>().ClosePanel(); // SceneTransitionOnProximity script
            Debug.Log("Correct Pattern! La puerta se ha abierto");
        }
        else
        {
            // incorrecto
            incorrectSound.Play();
            Debug.Log("Incorrect Pattern.");
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
