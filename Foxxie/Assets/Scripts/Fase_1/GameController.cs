using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject mundoN, mundoE;
    public Image cooldownDash, cooldownDim;
    public Material grayScale;

    // Start is called before the first frame update
    void Start()
    {
        mundoN.SetActive(true);
        mundoE.SetActive(false);
        grayScale.SetFloat("_GrayscaleAmount", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
