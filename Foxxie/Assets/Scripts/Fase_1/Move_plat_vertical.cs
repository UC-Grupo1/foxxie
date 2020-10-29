using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_plat_vertical : MonoBehaviour
{
    public Transform inicial, final;
    public float speed;

    public bool invert;

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if(!invert && transform.position == final.transform.position)
        {
            invert = true;
        }
        else if(invert && transform.position == inicial.transform.position)
        {
            invert = false;
        }

        transform.position = Vector2.MoveTowards(transform.position, invert ? inicial.transform.position : final.transform.position, speed);
    }
}
