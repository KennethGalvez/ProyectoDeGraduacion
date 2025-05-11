using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public PointsManager pointsManager;

    public void Nuevo(string nombreNivel)
    {
        MuteMenuAudio(); // 🔇 Silencia el audio del menú antes de cambiar de escena
        SceneManager.LoadScene(nombreNivel);
        pointsManager.ResetStats();
    }

    public void Creditos(string nombreNivel)
    {
        SceneManager.LoadScene(nombreNivel);
    }

    public void Opciones(string nombreNivel)
    {
        SceneManager.LoadScene(nombreNivel);
    }

    public void MenuPrincipal(string nombreNivel)
    {
        SceneManager.LoadScene(nombreNivel);
    }

    public void Salir()
    {
        Application.Quit();
    }

    private void MuteMenuAudio()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in audioSources)
        {
            audio.Stop();
        }
    }
}
