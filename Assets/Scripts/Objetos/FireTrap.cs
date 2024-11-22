using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    public Collider2D fireCollider; // Referencia al collider del fuego
    public Animator animator; // Referencia al Animator
    public ParticleSystem smokeParticle; // Sistema de partículas de humo
    public float fireOnTime = 2f; // Duración del fuego activo
    public float fireOffTime = 2f; // Duración del fuego inactivo
    public float smokeLeadTime = 1f; // Tiempo antes de que salga el fuego para que aparezca el humo

    public Transform teleportPoint; // Punto de teletransporte
    public Image fadeImage; // Imagen negra para el efecto de fade
    public float fadeDuration = 1f; // Duración de la transición de pantalla

    private bool isFireActive = false;

    private void Start()
    {
        StartCoroutine(FireCycle());
    }

    // Ciclo del fuego con humo previo
    private IEnumerator FireCycle()
    {
        while (true)
        {
            // Activar humo antes del fuego
            if (smokeParticle != null)
            {
                smokeParticle.gameObject.SetActive(true); // Activar el sistema de partículas
                smokeParticle.Play();
                Debug.Log("Humo activo");
            }

            yield return new WaitForSeconds(smokeLeadTime);

            // Apagar humo antes de encender el fuego
            if (smokeParticle != null)
            {
                smokeParticle.Stop();
                smokeParticle.gameObject.SetActive(false); // Desactivar el sistema de partículas
                Debug.Log("Humo desactivado");
            }

            // Activar el fuego
            isFireActive = true;
            fireCollider.enabled = true;
            animator.SetBool("fuego", true);
            Debug.Log("Fire is ON");

            // Esperar mientras el fuego está activo
            yield return new WaitForSeconds(fireOnTime);

            // Desactivar el fuego
            isFireActive = false;
            fireCollider.enabled = false;
            animator.SetBool("fuego", false);
            Debug.Log("Fire is OFF");

            // Esperar mientras el fuego está inactivo
            yield return new WaitForSeconds(fireOffTime);
        }
    }

    // Detectar colisión con el jugador
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFireActive && other.CompareTag("Player"))
        {
            Debug.Log("Player hit by fire!");
            StartCoroutine(HandlePlayerHit(other.gameObject));
        }
    }

    // Manejar colisión con el fuego
    private IEnumerator HandlePlayerHit(GameObject character)
    {
        // Congelar la pantalla
        Time.timeScale = 0f;
        yield return StartCoroutine(FadeToBlack());

        // Teletransportar al jugador
        if (teleportPoint != null)
        {
            character.transform.position = teleportPoint.position;
            Debug.Log("Player teleported to: " + teleportPoint.position);
        }
        else
        {
            Debug.LogWarning("Teleport point not set!");
        }

        // Reanudar la pantalla y transición de regreso
        yield return StartCoroutine(FadeToClear());
        Time.timeScale = 1f;
    }

    // Efecto de desvanecimiento a negro
    private IEnumerator FadeToBlack()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true); // Activar imagen negra
            float timer = 0f;
            Color color = fadeImage.color;

            while (timer < fadeDuration)
            {
                color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                fadeImage.color = color;
                timer += Time.unscaledDeltaTime; // Usar tiempo no escalado por pausa
                yield return null;
            }

            color.a = 1f;
            fadeImage.color = color;
        }
    }

    // Efecto de desvanecimiento a claro
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
                timer += Time.unscaledDeltaTime; // Usar tiempo no escalado por pausa
                yield return null;
            }

            color.a = 0f;
            fadeImage.color = color;
            fadeImage.gameObject.SetActive(false); // Desactivar imagen negra
        }
    }
}
