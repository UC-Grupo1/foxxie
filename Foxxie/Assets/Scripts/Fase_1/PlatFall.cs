using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatFall : MonoBehaviour
{
    public float tempo;

    private bool haveChildren;
    private float tempoDestroi;

    private void Start()
    {
        haveChildren = false;
        tempoDestroi = 1.5f;
    }

    void Update()
    {
        Fall();
    }

    private void Fall()
    {
        if(transform.childCount > 1)
        {
            haveChildren = true;
            GetComponent<Animator>().SetBool("Treme", true);
        }

        if(haveChildren)
        {
            tempo -= Time.deltaTime;

            if(tempo <= 0)
            {
                if(!TryGetComponent(out Rigidbody2D rb))
                {
                    gameObject.AddComponent<Rigidbody2D>();
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                }

                GetComponent<Animator>().SetBool("Play", true);
                GetComponent<Animator>().SetBool("Treme", false);

                tempoDestroi -= Time.deltaTime;

                if(tempoDestroi <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
