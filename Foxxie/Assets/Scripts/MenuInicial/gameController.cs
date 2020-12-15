using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class gameController : MonoBehaviour
{
    public List<GameObject> panels;

    private void Start()
    {
        TrocaMenu(0);
        SavePreferences a = new SavePreferences();
        a.Apply();
    }

    public void TrocaMenu(int menuAtual)
    {
        if(menuAtual == 0)
        {
            panels[0].SetActive(true);
            panels[1].SetActive(false);
        }
        else
        {
            panels[0].SetActive(false);
            panels[1].SetActive(true);
        }
    }

    public void IniciarJogo()
    {
        SceneManager.LoadScene("Fase_1");
    }

    public void Sair()
    {
        Application.Quit();
    }

    public void BtnRanking()
    {
        GameController.view = true;
        SceneManager.LoadScene("Highscore");
    }
}
