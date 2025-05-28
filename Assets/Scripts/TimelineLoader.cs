using UnityEngine;
using UnityEngine.Playables;

public class TimelineLoader : MonoBehaviour
{
    public PlayableDirector heartPathTimeline;
    public PlayableDirector mindPathTimeline;

    private void Start()
    {
        if (QuickTimeManager.dashUnlocked && heartPathTimeline != null)
        {
            Debug.Log("Activando Timeline del Corazón");
            heartPathTimeline.gameObject.SetActive(true);
            heartPathTimeline.Play();
        }
        else if (QuickTimeManager.doubleJumpUnlocked && mindPathTimeline != null)
        {
            Debug.Log("Activando Timeline de la Mente");
            mindPathTimeline.gameObject.SetActive(true);
            mindPathTimeline.Play();
        }
        else
        {
            Debug.LogWarning("No se encontró una decisión previa o no están asignados los timelines.");
        }
    }
}
