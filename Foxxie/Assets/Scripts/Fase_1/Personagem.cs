﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personagem : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public LayerMask GroundLayer, trocaDimensao, layerJump;
    public GameController gc;
    public bool isDash;
    public AudioSource audioDim;
    public List<AudioClip> efeitos;

    Rigidbody2D rb2D;
    BoxCollider2D boxCollider2D;
    bool isMundoE, isCDDash, isCDDim, onGround;
    float tempoDim, tempoDash, dashSpeed, timeInDash;
    int direction;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        isMundoE = false;
        tempoDim = 2f;
        tempoDash = 2f;
        direction = 0;
        dashSpeed = 25f;
        timeInDash = 0.25f;
        isDash = false;
        isCDDash = true;
        isCDDim = true;
        onGround = true;
    }

    void Update()
    {
        Move();
        Jump();
        TrocaDimensao();
        Dash();
        ValidaPlatMove();
    }

    private void Move()
    {
        float direct = Input.GetAxisRaw("Horizontal");
        
        if (direct > 0)
        {
            direction = 1;
            if(!ValidaCol(direction))
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);
            }
        }
        else if (direct < 0)
        {
            direction = 2;
            if (!ValidaCol(direction))
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
            }
        }
    }

    private void Jump()
    {
        RaycastHit2D rayC = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, .1f, layerJump);
        onGround = rayC.collider != null ? true : false;

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
            if(!ValidaTrocaDimensao())
            {
                gc.grayScale.SetFloat("_GrayscaleAmount", 0f);
                isMundoE = false;
                tempoDim = 2f;
                isCDDim = true;

                audioDim.clip = efeitos[0];
                audioDim.Play();
            }
            else
            {
                gc.mundoN.SetActive(false);
                gc.mundoE.SetActive(true);

                audioDim.clip = efeitos[1];
                audioDim.Play();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isMundoE && tempoDim <= 0)
        {
            gc.mundoN.SetActive(false);
            gc.mundoE.SetActive(true);
            if (!ValidaTrocaDimensao())
            {
                gc.grayScale.SetFloat("_GrayscaleAmount", 1f);
                isMundoE = true;
                tempoDim = 2f;
                isCDDim = true;

                audioDim.clip = efeitos[0];
                audioDim.Play();
            }
            else
            {
                gc.mundoN.SetActive(true);
                gc.mundoE.SetActive(false);

                audioDim.clip = efeitos[1];
                audioDim.Play();
            }
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

        if (direction == 1 && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && tempoDash <= 0)
        {
            rb2D.velocity = Vector2.right * dashSpeed;
            isDash = true;
            tempoDash = 2f;
            isCDDash = true;
        }
        else if (direction == 2 && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && tempoDash <= 0)
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

    private bool ValidaCol(int dir)
    {
        RaycastHit2D rayC = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, dir == 1 ? Vector2.right : Vector2.left, .1f, GroundLayer);
        return rayC.collider != null ? true : false;
    }

    private bool ValidaTrocaDimensao()
    {
        RaycastHit2D rayC = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.right, .05f, trocaDimensao);
        if(rayC.collider != null)
        {
            gc.animatorTrocaDim.SetBool("falha", true);
        }
        
        return rayC.collider != null ? true : false;
    }
    private void ValidaPlatMove()
    {
        RaycastHit2D rayC = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, .1f, layerJump);
        
        if(rayC.collider != null && rayC.collider.tag == "Plat_move")
        {
            transform.SetParent(rayC.transform);
        }
        else
        {
            transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "DeathZone")
        {
            rb2D.velocity = Vector2.zero;
            transform.position = gc.GetComponent<GameController>().initialPos.position;
        }
    }

}
