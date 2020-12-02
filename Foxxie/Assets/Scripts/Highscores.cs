using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Highscores : MonoBehaviour
{
    const string privateCode = "ktDTzmrCVE2CB7ZWKM9sDwhzwzJ8MKhkiRCns6SeKr9Q";
    const string publicCode = "5fbd22a4eb36fd2714e91b4c";
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;
    public InputField nome;
    public Text txtPontos;
    public GameObject panelNome, panelPontos, panelErro, primeiro, segundo, terceiro;
    public Button btnConfirm, btnAgain;
    [SerializeField]
    private int pontos;

    private void Awake()
    {
        pontos = (int)GameController.pontos;
        GetComponent<SavePreferences>().Apply();
    }

    private void Start()
    {
        panelNome.SetActive(true);
        panelPontos.SetActive(false);
        primeiro.SetActive(false);
        segundo.SetActive(false);
        terceiro.SetActive(false);
        txtPontos.text = pontos.ToString();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (GameController.view)
        {
            btnAgain.gameObject.SetActive(false);
            panelNome.SetActive(false);
            panelPontos.SetActive(true);
            DownloadHighscores();
        }
        else
        {
            btnAgain.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        btnConfirm.interactable = nome.text == null || nome.text == "" ? false : true;
    }

    public void AddNewHighscores(string username, int score)
    {
        StartCoroutine(UploadNewHighscores(username, score));
    }

    IEnumerator UploadNewHighscores(string username, int score)
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if(string.IsNullOrEmpty(www.error))
        {
            print("Score salvo!");
            panelErro.SetActive(false);
            DownloadHighscores();
        }
        else
        {
            ConfigError(www.error);
        }
    }

    public void DownloadHighscores()
    {
        StartCoroutine("DownloadHighscoresFromDataBase");
    }

    IEnumerator DownloadHighscoresFromDataBase()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            panelErro.SetActive(false);
            FormatterHighscores(www.text);

            if(highscoresList.Length >= 3)
            {
                primeiro.SetActive(true);
                primeiro.GetComponent<PosicaoTabela>().nome.text = highscoresList[0].username;
                primeiro.GetComponent<PosicaoTabela>().pontos.text = highscoresList[0].score.ToString();

                segundo.SetActive(true);
                segundo.GetComponent<PosicaoTabela>().nome.text = highscoresList[1].username;
                segundo.GetComponent<PosicaoTabela>().pontos.text = highscoresList[1].score.ToString();

                terceiro.SetActive(true);
                terceiro.GetComponent<PosicaoTabela>().nome.text = highscoresList[2].username;
                terceiro.GetComponent<PosicaoTabela>().pontos.text = highscoresList[2].score.ToString();
            }
            else if (highscoresList.Length >= 2)
            {
                primeiro.SetActive(true);
                primeiro.GetComponent<PosicaoTabela>().nome.text = highscoresList[0].username;
                primeiro.GetComponent<PosicaoTabela>().pontos.text = highscoresList[0].score.ToString();

                segundo.SetActive(true);
                segundo.GetComponent<PosicaoTabela>().nome.text = highscoresList[1].username;
                segundo.GetComponent<PosicaoTabela>().pontos.text = highscoresList[1].score.ToString();

                terceiro.SetActive(false);
            }
            else if (highscoresList.Length >= 1)
            {
                primeiro.SetActive(true);
                primeiro.GetComponent<PosicaoTabela>().nome.text = highscoresList[0].username;
                primeiro.GetComponent<PosicaoTabela>().pontos.text = highscoresList[0].score.ToString();

                segundo.SetActive(false);

                terceiro.SetActive(false);
            }
        }
        else
        {
            ConfigError(www.error);
        }
    }

    public void DeletarRegistro(string username, int score)
    {
        StartCoroutine(DeletaRegidtro(username, score));
    }

    IEnumerator DeletaRegidtro(string username, int score)
    {
        WWW www = new WWW(webURL + publicCode + "/delete/" + username);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            AddNewHighscores(username, score);
        }
        else
        {
            ConfigError(www.error);
        }
    }

    public void ValidarNomeExistente(string username, int score)
    {
        StartCoroutine(ValidaNomeExistente(username, score));
    }

    IEnumerator ValidaNomeExistente(string username, int score)
    {
        WWW www = new WWW(webURL + publicCode + "/pipe-get/" + username);
        yield return www;
        
        if(string.IsNullOrEmpty(www.error))
        {
            if(string.IsNullOrEmpty(www.text))
            {
                AddNewHighscores(username, score);
            }
            else
            {
                string[] registro = www.text.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (int.Parse(registro[1]) < score)
                {
                    DeletarRegistro(username, score);
                }
                else
                {
                    AddNewHighscores(username, score);
                }
            }
        }
        else
        {
            ConfigError(www.error);
        }
    }

    void FormatterHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresList = new Highscore[entries.Length];

        for(int i = 0; i < entries.Length; i++)
        {
            string[] entriesInfo = entries[i].Split(new char[] {'|'});
            string username = entriesInfo[0];
            int score = int.Parse(entriesInfo[1]);
            highscoresList[i] = new Highscore(username, score);
        }
    }

    public void ConfirmaPontos()
    {
        panelNome.SetActive(false);
        panelPontos.SetActive(true);

        ValidarNomeExistente(nome.text, pontos);
    }

    public void BtnVoltar()
    {
        SceneManager.LoadScene("MenuInicial");
    }

    public void BtnTentarNovamente()
    {
        SceneManager.LoadScene("Fase_1");
    }

    private void ConfigError(string erro)
    {
        panelErro.SetActive(true);
        panelNome.SetActive(false);
        panelPontos.SetActive(false);
        print("Erro: " + erro);
    }
}

public struct Highscore
{
    public string username;
    public int score;

    public Highscore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}
