using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public void Nuevo(string nombreNivel)
    {
        SceneManager.LoadScene(nombreNivel);
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
