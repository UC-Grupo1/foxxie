using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    public float speed;
    public int jumpForce;
    public LayerMask GroundLayer;
    public GameController gc;
    public bool isDash;
    public AudioSource audioDim;

    Rigidbody2D rb2D;
    BoxCollider2D boxCollider2D;
    bool isMundoE, isCDDash, isCDDim;
    float tempoDim, tempoDash, dashSpeed, timeInDash;
    int direction;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        //audioDim = GetComponent<AudioSource>();
        isMundoE = false;
        tempoDim = 2f;
        tempoDash = 2f;
        direction = 0;
        dashSpeed = 25f;
        timeInDash = 0.25f;
        isDash = false;
        isCDDash = true;
        isCDDim = true;
    }

    void Update()
    {
        Move();
        Jump();
        TrocaDimensao();
        Dash();
    }

    private void Move()
    {
        float direct = Input.GetAxisRaw("Horizontal");

        if (direct > 0)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);
            direction = 1;
        }
        else if (direct < 0)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
            direction = 2;
        }
    }

    private void Jump()
    {
        RaycastHit2D rayC = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, .1f, GroundLayer);

        if(rayC.collider != null && Input.GetKey(KeyCode.W))
        {
            rb2D.velocity = Vector2.up * jumpForce;
        }
    }

    private void TrocaDimensao()
    {
        if(tempoDim > 0)
        {
            tempoDim -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space) && isMundoE && tempoDim <= 0)
        {
            gc.mundoN.SetActive(true);
            gc.mundoE.SetActive(false);
            isMundoE = false;
            tempoDim = 2f;
            isCDDim = true;
            gc.grayScale.SetFloat("_GrayscaleAmount", 0f);
            audioDim.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isMundoE && tempoDim <= 0)
        {
            gc.mundoN.SetActive(false);
            gc.mundoE.SetActive(true);
            isMundoE = true;
            tempoDim = 2f;
            isCDDim = true;
            gc.grayScale.SetFloat("_GrayscaleAmount", 1f);
            audioDim.Play();
        }

        if (isCDDim)
        {
            gc.cooldownDim.fillAmount += 1 / 2f * Time.deltaTime;
            if (gc.cooldownDim.fillAmount >= 1)
            {
                gc.cooldownDim.fillAmount = 0;
                isCDDim = false;
            }
        }
    }

    private void Dash()
    {
        if (tempoDash > 0)
        {
            tempoDash -= Time.deltaTime;
        }

        if (direction == 1 && Input.GetKeyDown(KeyCode.LeftShift) && tempoDash <= 0)
        {
            rb2D.velocity = Vector2.right * dashSpeed;
            isDash = true;
            tempoDash = 2f;
            isCDDash = true;
        }
        else if (direction == 2 && Input.GetKeyDown(KeyCode.LeftShift) && tempoDash <= 0)
        {
            rb2D.velocity = Vector2.left * dashSpeed;
            isDash = true;
            tempoDash = 2f;
            isCDDash = true;
        }

        if(isDash)
        {
            timeInDash -= Time.deltaTime;

            if(timeInDash <= 0)
            {
                isDash = false;
                rb2D.velocity = Vector2.zero;
                timeInDash = 0.25f;
            }
        }

        if (isCDDash)
        {
            gc.cooldownDash.fillAmount += 1 / 2f * Time.deltaTime;
            if (gc.cooldownDash.fillAmount >= 1)
            {
                gc.cooldownDash.fillAmount = 0;
                isCDDash = false;
            }
        }
    }
}
