using UnityEngine;

public class PipelineMov : MonoBehaviour
{
    public Vector3 targetPosition;
    public float speed = 2f;
    public float respawnDelay = 1f;

    [Header("Blink Settings")]
    public float blinkDistance = 0.3f;         // 🟡 Distancia antes del destino donde empieza a titilar
    public float blinkDuration = 0.5f;         // 🔴 Duración total del titileo
    public float blinkInterval = 0.1f;         // 🔁 Intervalo entre on/off visual

    private Vector3 startPosition;
    private bool isMoving = true;
    private bool isBlinking = false;

    private Renderer platformRenderer;

    private void Start()
    {
        startPosition = transform.position;
        platformRenderer = GetComponent<Renderer>();

        if (platformRenderer == null)
            Debug.LogWarning("Renderer no encontrado en la plataforma.");
    }

    private void Update()
    {
        if (isMoving)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        float remainingDistance = Vector3.Distance(transform.position, targetPosition);

        // 🔁 Inicia el parpadeo un poco antes de llegar
        if (remainingDistance <= blinkDistance && !isBlinking)
        {
            isBlinking = true;
            StartCoroutine(BlinkAndDestroy());
        }

        // 🛑 Evita que se destruya dos veces
        if (transform.position == targetPosition && !isBlinking)
        {
            isMoving = false;
            DestroyPlatform();
        }
    }

    private System.Collections.IEnumerator BlinkAndDestroy()
    {
        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < blinkDuration)
        {
            if (platformRenderer != null)
                platformRenderer.enabled = isVisible;

            isVisible = !isVisible;
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        if (platformRenderer != null)
            platformRenderer.enabled = true;

        isMoving = false;
        DestroyPlatform();
    }

    private void DestroyPlatform()
    {
        gameObject.SetActive(false);
        Invoke(nameof(RespawnPlatform), respawnDelay);
    }

    private void RespawnPlatform()
    {
        transform.position = startPosition;
        gameObject.SetActive(true);
        isMoving = true;
        isBlinking = false; // ✅ Reinicia el estado
    }
}
