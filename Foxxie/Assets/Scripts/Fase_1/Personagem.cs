using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Personagem : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public LayerMask GroundLayer, trocaDimensao, layerJump;
    public GameController gc;
    public bool isDash, isDeath, isMundoE;
    public AudioSource audioDim;
    public List<AudioClip> efeitos;
    public int direction;

    Rigidbody2D rb2D;
    BoxCollider2D boxCollider2D;
    Animator anim;
    bool isCDDash, isCDDim, onGround;
    float tempoDim, tempoDash, dashSpeed, timeInDash, direct;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
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
        isDeath = false;
    }

    void Update()
    {
        AcoesPersonagem();
        ValidaPlatMove();
        ControlaAnimacao();
        checkPos();
    }

    private void AcoesPersonagem()
    {
        if(!isDeath)
        {
            Move();
            Jump();
            TrocaDimensao();
            Dash();
        }
    }

    private void Move()
    {
        direct = Input.GetAxisRaw("Horizontal");
        
        if (direct > 0)
        {
            direction = 1;
            if(!ValidaCol(direction, GroundLayer))
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);
                transform.eulerAngles = new Vector3(0,0,0);
            }
        }
        else if (direct < 0)
        {
            direction = 2;
            if (!ValidaCol(direction, GroundLayer))
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
                transform.eulerAngles = new Vector3(0, 180, 0);
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

        if(Input.GetKeyDown(KeyCode.Space) && isMundoE && tempoDim <= 0 || isDeath)
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && tempoDash <= 0)
        {
            rb2D.velocity = (direction == 1 ? Vector2.right : Vector2.left) * dashSpeed;
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

    private bool ValidaCol(int dir, LayerMask layer)
    {
        RaycastHit2D rayC = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, dir == 1 ? Vector2.right : Vector2.left, .1f, layer);
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

    private void ControlaAnimacao()
    {
        anim.SetBool("Run", direct != 0 ? true : false);
        anim.SetBool("onGround", onGround ? true : false);
        anim.SetInteger("inAir", (int)rb2D.velocity.y);
        if(isDeath)
        {
            anim.SetBool("death", true);
        }
    }

    public void ValidaMorteAnim()
    {
        anim.SetBool("death", false);
        rb2D.velocity = Vector2.zero;
        gameObject.transform.position = gc.initialPos.position;
        TrocaDimensao();
        isDeath = false;
    }

    private void checkPos()
    {
        if (gameObject.transform.GetChild(0).localPosition != Vector3.zero)
        {
            gameObject.transform.GetChild(0).localPosition = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "DeathZone" && !isDeath)
        {
            //rb2D.velocity = Vector2.zero;
            //transform.position = gc.GetComponent<GameController>().initialPos.position;
            isDeath = true;
            isDash = false;
            rb2D.velocity = Vector2.zero;
            isMundoE = false;

            audioDim.clip = efeitos[2];
            audioDim.Play();
        }

        if (col.gameObject.tag == "Moeda")
        {
            gc.moedas++;
            Destroy(col.gameObject);

            audioDim.clip = efeitos[3];
            audioDim.Play();
        }

        if (col.gameObject.tag == "Chave")
        {
            gc.pegouChave = true;
            Destroy(col.gameObject);
            gc.txtChave.text = "1 / 1";

            audioDim.clip = efeitos[4];
            audioDim.Play();
        }

        if (col.gameObject.tag == "Portal")
        {
            if(gc.pegouChave)
            {
                gc.CalculaPontos();
                GameController.view = false;
                SceneManager.LoadScene(2);
            }
        }

        if (col.gameObject.tag == "Checkpoint")
        {
            gc.checkpoint = true;
        }

        if(col.gameObject.tag == "StopCam")
        {
            gc.cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 1f;
            gc.cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 1f;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "StopCam")
        {
            gc.cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0f;
            gc.cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0f;
        }
    }

}
