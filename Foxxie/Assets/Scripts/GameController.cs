using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public static float pontos;
    public static bool view;

    public GameObject mundoN, mundoE, menu, fogo, background;
    public Image cooldownDash, cooldownDim;
    public Material grayScale;
    public Animator animatorTrocaDim;
    public Transform initialPos, tCheckpoint;
    public int moedas;
    public Text txtMoedas, txtTempo, txtChave;
    public bool pegouChave, checkpoint;
    public CinemachineVirtualCamera cam;

    bool inMenu;
    private float timer;

    void Start()
    {
        mundoN.SetActive(true);
        mundoE.SetActive(false);
        menu.SetActive(false);
        grayScale.SetFloat("_GrayscaleAmount", 0f);
        inMenu = false;
        pegouChave = false;

        SavePreferences a = new SavePreferences();
        a.Apply();
        moedas = 0;
        txtMoedas.text = "0 / 26";
        txtTempo.text = "0";
        txtChave.text = "0 / 1";

        timer = 0f;
        checkpoint = false;
    }

    void Update()
    {
        Acoes();
        AtualizaMoedas();
        ContaTempoFase();
        ValidaCheckpoint();
        AcendeFogo();
        SetaCinzaBackground();
        Configuracoes();
    }

    private void Acoes()
    {
        if(inMenu)
        {
            Time.timeScale = 0;
            menu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            menu.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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

    private void AtualizaMoedas()
    {
        txtMoedas.text = moedas.ToString() + " / 26";
    }

    private void ContaTempoFase()
    {
        timer += Time.deltaTime;
        //CalculaPontos(timer);
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        txtTempo.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void ValidaCheckpoint()
    {
        if(checkpoint)
        {
            initialPos = tCheckpoint;
        }
    }

    private void AcendeFogo()
    {
        if(checkpoint)
        {
            fogo.SetActive(true);
        }
    }

    private void SetaCinzaBackground()
    {
        bool mundoE = GameObject.FindWithTag("Player").GetComponent<Personagem>().isMundoE;
        Material mat = Resources.Load<Material>("Material/Fase_1/grayScale");

        for(int i = 0; i < background.transform.childCount; i++)
        {
            if(mundoE)
            {
                if(background.transform.GetChild(i).TryGetComponent(out LightRays2D light))
                {
                    background.transform.GetChild(i).GetComponent<LightRays2D>().color1 = new Color32(191, 191, 191, 255);
                    background.transform.GetChild(i).GetComponent<LightRays2D>().color2 = new Color32(71, 71, 71, 255);
                }
                else
                {
                    background.transform.GetChild(i).GetComponent<SpriteRenderer>().material = mat;
                }
            }
            else
            {
                if (background.transform.GetChild(i).TryGetComponent(out LightRays2D light))
                {
                    background.transform.GetChild(i).GetComponent<LightRays2D>().color1 = new Color32(248, 255, 26, 255);
                    background.transform.GetChild(i).GetComponent<LightRays2D>().color2 = new Color32(255, 169, 0, 255);
                }
            }
        }

        foreach(GameObject es in GameObject.FindGameObjectsWithTag("DeathZone"))
        {
            try
            {
                es.transform.GetChild(0).GetComponent<SpriteRenderer>().material = mat;
            }
            catch
            {
                continue;
            }
        }
    }

    public void CalculaPontos()
    {
        if(pontos >= 0)
        {
            pontos = (moedas * 10) / Mathf.FloorToInt(timer % 60) + 100;
            if(pontos < 0)
            {
                pontos = 0;
            }
        }
        print(pontos);
    }

    private void Configuracoes()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Fase_1");
        }
    }
}
