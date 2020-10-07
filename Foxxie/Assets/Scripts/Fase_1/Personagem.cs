using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    public float speed;
    public int jumpForce;
    Rigidbody2D rb2D;
    BoxCollider2D boxCollider2D;
    public LayerMask GroundLayer;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float direct = Input.GetAxisRaw("Horizontal");

        if (direct > 0)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);
        }
        else if (direct < 0)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
        }
    }

    private void Jump()
    {
        RaycastHit2D rayC = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, .1f, GroundLayer);

        if(rayC.collider != null && Input.GetKey(KeyCode.Space))
        {
            rb2D.velocity = Vector2.up * jumpForce;
        }
    }

}
