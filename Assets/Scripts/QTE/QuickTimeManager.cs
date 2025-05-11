using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class QuickTimeManager : MonoBehaviour
{
    public PointsManager pointsManager;

    public PlayableDirector currentTimeline;     // Timeline activo al iniciar
    public PlayableDirector heartPathTimeline;   // Timeline Q
    public PlayableDirector mindPathTimeline;    // Timeline E

    private bool eventCompleted = false;

    public static bool dashUnlocked = false;
    public static bool doubleJumpUnlocked = false;

    private void Start()
    {
        DestroyExtraEventSystems();
        DisableExtraAudioListeners();

        if (currentTimeline != null)
        {
            currentTimeline.gameObject.SetActive(true);
            currentTimeline.Play();
        }
    }

    private void Update()
    {
        if (!eventCompleted && currentTimeline != null && currentTimeline.state == PlayState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                pointsManager.AddHeartPoints(1);
                dashUnlocked = true;
                InterruptTimeline(heartPathTimeline, goToNivel2: true);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                pointsManager.AddMindPoints(1);
                doubleJumpUnlocked = true;
                InterruptTimeline(mindPathTimeline, goToNivel2: true);
            }
        }
    }

    private void InterruptTimeline(PlayableDirector nextTimeline, bool goToNivel2 = false)
    {
        // Detener y desactivar el timeline actual
        currentTimeline.Stop();
        currentTimeline.gameObject.SetActive(false);

        // Activar y reproducir el siguiente timeline
        if (nextTimeline != null)
        {
            nextTimeline.gameObject.SetActive(true);
            nextTimeline.Play();

            if (goToNivel2)
            {
                nextTimeline.stopped += OnMindPathTimelineFinished;
            }
        }

        eventCompleted = true;
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
