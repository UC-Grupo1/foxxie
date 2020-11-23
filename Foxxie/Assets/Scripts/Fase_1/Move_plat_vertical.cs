using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_plat_vertical : MonoBehaviour
{
    public Transform inicial, final;
    public float speed;
    public bool invert;

    private float timer;

    private void Start()
    {
        timer = 0.5f;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if(!invert && transform.position == final.transform.position)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                invert = true;
                timer = 0.5f;
            }
        }
        else if(invert && transform.position == inicial.transform.position)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                invert = false;
                timer = 0.5f;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, invert ? inicial.transform.position : final.transform.position, speed * Time.deltaTime);
    }
}
