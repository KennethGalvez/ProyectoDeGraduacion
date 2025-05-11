using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public PointsManager pointsManager;
    public void Nuevo()
    {
        CargaNivel.NivelCarga("CinematicaIntro");
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
}
