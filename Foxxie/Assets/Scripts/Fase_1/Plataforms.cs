using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforms : MonoBehaviour
{
    public float waitTime;
    private PlatformEffector2D effector;

    private void Start()
    {
        waitTime = 0.010f;
        effector = GetComponent<PlatformEffector2D>();
        SetAjustes();
    }

    private void Update()
    {
        Efeito();
    }

    private void Efeito()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            waitTime = 0.010f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = 0.010f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            effector.rotationalOffset = 0;
        }
    }

    private void SetAjustes()
    {
        if (gameObject.transform.parent.tag == "Mundo_espiritual")
        {
            Material mat = Resources.Load<Material>("Material/Fase_1/grayScale");
            gameObject.GetComponent<SpriteRenderer>().material = mat;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().material = mat;
        }
    }
}
