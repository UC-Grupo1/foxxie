using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameController : MonoBehaviour
{

    public GameObject mundoN, mundoE, menu, fogo, background;
    public Image cooldownDash, cooldownDim;
    public Material grayScale;
    public Animator animatorTrocaDim;
    public Transform initialPos, tCheckpoint;
    public int moedas;
    public Text txtMoedas, txtTempo;
    public bool pegouChave, checkpoint;
    public CinemachineVirtualCamera cam;

    bool inMenu;
    private float timer;

    // Start is called before the first frame update
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

        timer = 0f;
        checkpoint = false;
    }

    // Update is called once per frame
    void Update()
    {
        Acoes();
        AtualizaMoedas();
        ContaTempoFase();
        ValidaCheckpoint();
        AcendeFogo();
        SetaCinzaBackground();
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

    private void AtualizaMoedas()
    {
        txtMoedas.text = moedas.ToString() + " / 26";
    }

    private void ContaTempoFase()
    {
        timer += Time.deltaTime;
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
                background.transform.GetChild(i).GetComponent<SpriteRenderer>().material = mat;
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
}
