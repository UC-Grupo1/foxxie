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
        player = GetComponent<Personagem>();
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
                Destroy(gameObj, 4f);
                timeBtwSpawns = startTimeBtwSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }
}
