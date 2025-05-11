using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class QuickTimeManager : MonoBehaviour
{
    public PointsManager pointsManager;

    public PlayableDirector heartPathTimeline;  // Timeline para el camino del corazón (Q)
    public PlayableDirector mindPathTimeline;   // Timeline para el camino de la mente (E)

    private bool eventCompleted = false;

    public static bool dashUnlocked = false;
    public static bool doubleJumpUnlocked = false;

    private void Start()
    {
        DestroyExtraEventSystems();
        DisableExtraAudioListeners();
    }

    private void Update()
    {
        if (!eventCompleted)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                pointsManager.AddHeartPoints(1);
                dashUnlocked = true;
                ActivateTimeline(heartPathTimeline);
                eventCompleted = true;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                pointsManager.AddMindPoints(1);
                doubleJumpUnlocked = true;
                ActivateTimeline(mindPathTimeline, goToNivel2: true);
                eventCompleted = true;
            }
        }
    }

    private void ActivateTimeline(PlayableDirector timeline, bool goToNivel2 = false)
    {
        if (timeline != null)
        {
            timeline.Play();

            if (goToNivel2)
            {
                timeline.stopped += OnMindPathTimelineFinished;
            }
        }
        else
        {
            Debug.LogWarning("No se asignó un Timeline para este camino.");
        }
    }

    private void OnMindPathTimelineFinished(PlayableDirector director)
    {
        SceneManager.LoadScene("Nivel 2");
    }

    private void DestroyExtraEventSystems()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);
            }
        }
    }

    private void DisableExtraAudioListeners()
    {
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();
        if (audioListeners.Length > 1)
        {
            for (int i = 1; i < audioListeners.Length; i++)
            {
                audioListeners[i].enabled = false;
            }
        }
    }
}
