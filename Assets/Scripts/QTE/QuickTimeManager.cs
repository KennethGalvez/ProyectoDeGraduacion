using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickTimeManager : MonoBehaviour
{
    public PlayableDirector introTimeline;
    public PlayableDirector heartPathTimeline;
    public PlayableDirector mindPathTimeline;

    public static bool dashUnlocked = false;
    public static bool doubleJumpUnlocked = false;

    [Header("Decision Settings")]
    public float decisionTime = 15f;
    private float decisionTimer;
    private bool decisionPhase = false;
    private int currentSelection = 0;

    [Header("UI Elements")]
    public Image heartImage;
    public Image mindImage;
    public float scaleMultiplier = 1.2f;
    private Vector3 heartOriginalScale;
    private Vector3 mindOriginalScale;

    [Header("Points")]
    public PointsManager pointsManager;
    public int pointsPerDecision = 1;
    public float progressIncrement = 0.1f;  // Cuánto sumar por decisión

    private void Start()
    {
        // Iniciar Timeline inicial
        if (introTimeline != null)
        {
            introTimeline.stopped += OnIntroTimelineFinished;
            introTimeline.gameObject.SetActive(true);
            introTimeline.Play();
        }

        if (heartImage != null)
            heartOriginalScale = heartImage.transform.localScale;
        if (mindImage != null)
            mindOriginalScale = mindImage.transform.localScale;

        decisionTimer = decisionTime;
        UpdateSelectionVisuals();
    }

    private void OnIntroTimelineFinished(PlayableDirector director)
    {
        decisionPhase = true;
    }

    private void Update()
    {
        if (decisionPhase)
        {
            HandleDecisionInput();
            decisionTimer -= Time.deltaTime;

            if (decisionTimer <= 0f)
            {
                ProcessDecision();
            }
        }
    }

    private void HandleDecisionInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentSelection = 0;
            UpdateSelectionVisuals();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentSelection = 1;
            UpdateSelectionVisuals();
        }
    }

    private void UpdateSelectionVisuals()
    {
        if (heartImage != null && mindImage != null)
        {
            heartImage.transform.localScale = (currentSelection == 0) ? heartOriginalScale * scaleMultiplier : heartOriginalScale;
            mindImage.transform.localScale = (currentSelection == 1) ? mindOriginalScale * scaleMultiplier : mindOriginalScale;
        }
    }

    private void ProcessDecision()
    {
        decisionPhase = false;

        if (currentSelection == 0)
        {
            dashUnlocked = true;
            pointsManager.AddHeartPoints(pointsPerDecision);

            // 🚀 Usar ProgressBarManager
            if (ProgressBarManager.Instance != null)
                ProgressBarManager.Instance.AddHeartProgress(progressIncrement);

            ActivateTimeline(heartPathTimeline);
        }
        else
        {
            doubleJumpUnlocked = true;
            pointsManager.AddMindPoints(pointsPerDecision);

            if (ProgressBarManager.Instance != null)
                ProgressBarManager.Instance.AddMindProgress(progressIncrement);

            ActivateTimeline(mindPathTimeline);
        }
    }

    private void ActivateTimeline(PlayableDirector timeline)
    {
        if (timeline != null)
        {
            timeline.gameObject.SetActive(true);
            timeline.Play();
            timeline.stopped += OnTimelineFinished;
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        SceneManager.LoadScene("Nivel 2");
    }
}
