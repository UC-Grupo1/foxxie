using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sons : MonoBehaviour
{
    public List<AudioClip> audios;

    public void TocaSomButton()
    {
        GetComponent<AudioSource>().clip = audios[0];
        GetComponent<AudioSource>().Play();
    }
}
