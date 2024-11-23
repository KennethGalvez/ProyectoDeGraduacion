using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpikeTrap : MonoBehaviour
{
    public Transform teleportPoint; // Punto de teletransporte
    public Image fadeImage; // Imagen negra para el efecto de fade
    public float fadeDuration = 1f; // Duración de la transición de pantalla

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit the spikes! Teleporting...");
            StartCoroutine(HandlePlayerHit(other.gameObject));
        }
    }

    // Manejar colisión con los pinchos y aplicar efecto de fade
    private IEnumerator HandlePlayerHit(GameObject character)
    {
        // Activar fade a negro
        yield return StartCoroutine(FadeToBlack());

        // Teletransportar al jugador
        if (teleportPoint != null)
        {
            character.transform.position = teleportPoint.position;
            Debug.Log("Player teleported to: " + teleportPoint.position);
        }
        else
        {
            Debug.LogWarning("Teleport point not set for the SpikeTrap!");
        }

        // Volver a aclarar la pantalla
        yield return StartCoroutine(FadeToClear());
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
                timer += Time.unscaledDeltaTime;
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
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            color.a = 0f;
            fadeImage.color = color;
            fadeImage.gameObject.SetActive(false); // Desactivar imagen negra
        }
    }
}
