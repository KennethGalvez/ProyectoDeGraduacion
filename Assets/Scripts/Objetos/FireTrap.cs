using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    public Collider2D fireCollider;
    public GameObject fireObject;
    public ParticleSystem smokeParticle;
    public float fireOnTime = 2f;
    public float fireOffTime = 2f;
    public float smokeLeadTime = 1f;

    public Transform teleportPoint;
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Audio")]
    public AudioSource fireSound; // 🔊 Sonido de fuego (asignar en inspector)

    private bool isFireActive = false;

    private void Start()
    {
        StartCoroutine(FireCycle());
    }

    private void Update()
    {
        if (isFireActive)
        {
            CheckForPlayerHit();
        }
    }

    private IEnumerator FireCycle()
    {
        while (true)
        {
            if (smokeParticle != null)
            {
                smokeParticle.gameObject.SetActive(true);
                smokeParticle.Play();
                Debug.Log("Smoke ON");
            }

            yield return new WaitForSeconds(smokeLeadTime);

            if (smokeParticle != null)
            {
                smokeParticle.Stop();
                smokeParticle.gameObject.SetActive(false);
                Debug.Log("Smoke OFF");
            }

            // Activar fuego y sonido
            isFireActive = true;
            fireCollider.enabled = true;

            if (fireObject != null)
            {
                fireObject.SetActive(true);
                Debug.Log("Fire ON");
            }

            if (fireSound != null)
            {
                fireSound.Play();
            }

            yield return new WaitForSeconds(fireOnTime);

            // Desactivar fuego y sonido
            isFireActive = false;
            fireCollider.enabled = false;

            if (fireObject != null)
            {
                fireObject.SetActive(false);
                Debug.Log("Fire OFF");
            }

            if (fireSound != null)
            {
                fireSound.Stop();
            }

            yield return new WaitForSeconds(fireOffTime);
        }
    }


    public LayerMask playerLayer; // Assign this in Inspector

    private void CheckForPlayerHit()
    {
        if (!isFireActive) return;

        Collider2D hit = Physics2D.OverlapBox(fireCollider.bounds.center, fireCollider.bounds.size, 0f, playerLayer);
        if (hit != null && hit.CompareTag("Player"))
        {
            Debug.Log("Player hit manually detected!");
            StartCoroutine(HandlePlayerHit(hit.gameObject));
        }
    }


    private IEnumerator HandlePlayerHit(GameObject character)
    {
        Player playerScript = character.GetComponent<Player>();
        if (playerScript != null)
        {
            yield return playerScript.TakeHitAndRecover();
        }

        Time.timeScale = 0f;
        yield return StartCoroutine(FadeToBlack());

        if (teleportPoint != null)
        {
            character.transform.position = teleportPoint.position;
            Debug.Log("Player teleported to: " + teleportPoint.position);
        }
        else
        {
            Debug.LogWarning("Teleport point not set!");
        }

        yield return StartCoroutine(FadeToClear());
        Time.timeScale = 1f;
    }


    private IEnumerator FadeToBlack()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            float timer = 0f;
            Color color = fadeImage.color;

            while (timer < fadeDuration)
            {
                color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                fadeImage.color = color;
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            color.a = 1f;
            fadeImage.color = color;
        }
    }

    private IEnumerator FadeToClear()
    {
        if (fadeImage != null)
        {
            float timer = 0f;
            Color color = fadeImage.color;

            while (timer < fadeDuration)
            {
                color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                fadeImage.color = color;
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            color.a = 0f;
            fadeImage.color = color;
            fadeImage.gameObject.SetActive(false);
        }
    }
}
