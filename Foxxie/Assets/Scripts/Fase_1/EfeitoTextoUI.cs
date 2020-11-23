using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeitoTextoUI : MonoBehaviour
{
    private GameObject player;
    private bool block;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        this.block = true;
    }

    // Update is called once per frame
    void Update()
    {
        Efeito();
    }

    private void Efeito()
    {
        if(this.block)
        {
            float dis = 255 - (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) * 15);
            Debug.Log(this.gameObject.name + ": " + dis);
            if(dis >= 0)
            {
                this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)dis);
            }
            else
            {
                this.block = false;
            }
        }
    }
}
