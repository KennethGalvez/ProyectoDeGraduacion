using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LavaController : MonoBehaviour
{
    public Transform lavaTransform;         // Lava que sube
    public float riseSpeed = 2f;            // Velocidad de subida
    public float maxHeight = 10f;           // Altura máxima de la lava
    public AudioSource alarmSound;          // Sonido de alarma

    public Image fadeImage;                 // Imagen para el efecto de fade
    public Transform safePosition;         // Posición segura a la que vuelve el jugador

    [Header("Trigger Reset")]
    public GameObject lavaTrigger;         // Trigger que inicia la lava

    private bool isLavaActive = false;
    private Vector3 initialLavaPosition;   // ✅ Guarda la posición inicial

    private void Start()
    {
        initialLavaPosition = lavaTransform.position;  // ✅ Guarda al iniciar
    }

    private void Update()
    {
        if (isLavaActive && lavaTransform.position.y < maxHeight)
        {
            lavaTransform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
        }
    }

    public void ActivateLava()
    {
        isLavaActive = true;
        if (alarmSound != null)
        {
            alarmSound.Stop();    // Reinicia la alarma si ya estaba sonando
            alarmSound.Play();
        }
    }

    public void HandlePlayerHit(GameObject player)
    {
        StartCoroutine(ResetPlayer(player));
    }

    private IEnumerator ResetPlayer(GameObject player)
    {
        Time.timeScale = 0f;
        yield return StartCoroutine(FadeToBlack());

        player.transform.position = safePosition.position;

        ResetEnvironment();

        yield return StartCoroutine(FadeToClear());
        Time.timeScale = 1f;
    }

    private void ResetEnvironment()
    {
        lavaTransform.position = initialLavaPosition;  // ✅ Restaura posición original
        isLavaActive = false;

        if (alarmSound != null)
            alarmSound.Stop();

        if (lavaTrigger != null)
            lavaTrigger.SetActive(true);  // ✅ Reactiva el trigger
    }

    private IEnumerator FadeToBlack()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            float timer = 0f;
            Color color = fadeImage.color;
            while (timer < 1f)
            {
                color.a = Mathf.Lerp(0f, 1f, timer / 1f);
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
            while (timer < 1f)
            {
                color.a = Mathf.Lerp(1f, 0f, timer / 1f);
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
