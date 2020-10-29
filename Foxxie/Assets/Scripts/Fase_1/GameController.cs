using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject mundoN, mundoE, menu;
    public Image cooldownDash, cooldownDim;
    public Material grayScale;
    public Animator animatorTrocaDim;
    public Transform initialPos;

    bool inMenu;

    // Start is called before the first frame update
    void Start()
    {
        mundoN.SetActive(true);
        mundoE.SetActive(false);
        menu.SetActive(false);
        grayScale.SetFloat("_GrayscaleAmount", 0f);
        inMenu = false;

        SavePreferences a = new SavePreferences();
        a.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        Acoes();
    }

    private void Acoes()
    {
        if(inMenu)
        {
            Time.timeScale = 0;
            menu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            menu.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            inMenu = true;
        }
    }

    public void CloseMenu()
    {
        inMenu = false;
    }

    public void BackMenuIncial()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}
