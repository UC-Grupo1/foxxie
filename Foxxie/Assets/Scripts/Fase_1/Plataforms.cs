using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforms : MonoBehaviour
{
    public float waitTime;
    private PlatformEffector2D effector;

    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        Efeito();
    }

    private void Efeito()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            waitTime = 0.01f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = 0.01f;
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
}
