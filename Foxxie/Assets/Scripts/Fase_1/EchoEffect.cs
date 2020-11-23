using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    public float startTimeBtwSpawns;
    private float timeBtwSpawns;
    public GameObject echo;
    private Personagem player;

    private void Start()
    {
        player = gameObject.transform.parent.GetComponent<Personagem>();
    }

    private void Update()
    {
        Spawns();
    }

    private void Spawns()
    {
        if(player.isDash)
        {
            if(timeBtwSpawns <= 0)
            {
                GameObject gameObj = (GameObject)Instantiate(echo, transform.position, Quaternion.identity);
                gameObj.GetComponent<SpriteRenderer>().sprite = player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                if(player.direction == 2)
                {
                    gameObj.transform.eulerAngles = new Vector3(0, 180, 0);
                }
                Destroy(gameObj, 4f);
                timeBtwSpawns = startTimeBtwSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }

    public void death()
    {
        player.ValidaMorteAnim();
    }
}
