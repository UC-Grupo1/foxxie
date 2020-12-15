using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatFall : MonoBehaviour
{
    public float tempo, realTempoRespawn, tempoRespawn;

    private bool haveChildren;
    private float tempoDestroi;
    private Vector2 posIni;

    private void Start()
    {
        haveChildren = false;
        tempoDestroi = 1.5f;
        posIni = gameObject.transform.position;
        tempoRespawn = realTempoRespawn;
    }

    void Update()
    {
        Fall();
        Cinza();
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
                        plat.transform.localScale = new Vector3(0.75f, 1, 1);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private void Cinza()
    {
        if (GameObject.FindWithTag("Player").GetComponent<Personagem>().isMundoE)
        {
            Material mat = Resources.Load<Material>("Material/Fase_1/grayScale");
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().material = mat;
        }
    }
}
