using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatFall : MonoBehaviour
{
    public float tempo, realTempoRespawn;

    private bool haveChildren;
    private float tempoDestroi, tempoRespawn;
    private Vector2 posIni;

    private void Start()
    {
        haveChildren = false;
        tempoDestroi = 1.5f;
        posIni = gameObject.transform.position;
        tempoRespawn = realTempoRespawn;
        SetAjustes();
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
                    tempoRespawn -= Time.deltaTime;

                    if(tempoRespawn <= 0)
                    {
                        GameObject plat = Instantiate(Resources.Load<GameObject>("Prefabs/Fase_1/platFall"), posIni, Quaternion.identity, gameObject.transform.parent);
                        Destroy(gameObject);
                    }
                }
            }
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
