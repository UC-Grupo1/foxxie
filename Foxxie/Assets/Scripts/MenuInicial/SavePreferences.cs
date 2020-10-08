using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SavePreferences : MonoBehaviour
{

    public Slider volume;
    public Dropdown resolucao, telaCheia;

    public void Save()
    {
        string path = Application.dataPath + "/Resources/Preferences.txt";
        if(!File.Exists(path))
        {
            File.Create(path);
        }

        File.WriteAllText(path, string.Empty);
        string content = "volume: " + volume.value + "\n";
        content += "resolucao: " + resolucao.value + "|" + resolucao.options[resolucao.value].text + "\n";
        content += "telaCheia: " + telaCheia.value;

        File.AppendAllText(path, content);

        Apply();
    }

    public void Apply()
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/Resources/Preferences.txt");
        List<string> linha = new List<string>();

        while (reader.Peek() >= 0)
        {
            linha.Add(reader.ReadLine().Split(':')[1].Trim());
        }

        GameObject.Find("Canvas").GetComponentsInChildren<Slider>(true)[0].value = float.Parse(linha[0]);
        GameObject.Find("Canvas").GetComponentsInChildren<Dropdown>(true)[0].value = int.Parse(linha[1].Split('|')[0]);
        GameObject.Find("Canvas").GetComponentsInChildren<Dropdown>(true)[1].value = int.Parse(linha[2]);

        Object[] objs = GameObject.FindObjectsOfType(typeof(AudioSource), true);
        foreach (AudioSource obj in objs)
        {
            obj.volume = float.Parse(linha[0]);
        }

        string[] resolucao = linha[1].Split('|')[1].Split('x');
        Screen.SetResolution(int.Parse(resolucao[0]), int.Parse(resolucao[1]), linha[2] == "0" ? true : false);

        reader.Close();
    }
}
