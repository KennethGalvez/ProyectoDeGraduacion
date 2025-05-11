using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    public LavaController lavaController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lavaController.ActivateLava();
            gameObject.SetActive(false); // Desactiva el trigger para que no se reactive múltiples veces
        }
    }
}
