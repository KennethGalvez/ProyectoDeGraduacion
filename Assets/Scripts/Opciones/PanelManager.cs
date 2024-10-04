using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // No destruir este objeto al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe una instancia, destruir el objeto duplicado
            Destroy(gameObject);
        }
    }
}
