using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    public static ProgressBarManager Instance;  // Singleton para persistencia

    [Header("Progress Bars")]
    public Slider heartProgressBar;
    public Slider mindProgressBar;

    [Header("Progress Values")]
    public static float heartProgress = 0f;
    public static float mindProgress = 0f;

    [Header("Configuración")]
    public float maxProgress = 1f;

    private void Awake()
    {
        // Configurar singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }

    private void Start()
    {
        UpdateProgressBars();
    }

    public void AddHeartProgress(float amount)
    {
        heartProgress = Mathf.Clamp(heartProgress + amount, 0, maxProgress);
        UpdateProgressBars();
    }

    public void AddMindProgress(float amount)
    {
        mindProgress = Mathf.Clamp(mindProgress + amount, 0, maxProgress);
        UpdateProgressBars();
    }

    private void UpdateProgressBars()
    {
        if (heartProgressBar != null)
            heartProgressBar.value = heartProgress;
        if (mindProgressBar != null)
            mindProgressBar.value = mindProgress;
    }

    public void ResetProgress()
    {
        heartProgress = 0f;
        mindProgress = 0f;
        UpdateProgressBars();
    }
}
