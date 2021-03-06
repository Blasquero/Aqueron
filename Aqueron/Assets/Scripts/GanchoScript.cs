﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GanchoScript : MonoBehaviour {
    private Rigidbody2D playerRb;
    private GameObject player;
    private bool activeScythe;
    private bool shouldMove;
    private bool hit;
    private bool aerialHook = true;
    private bool activeScytheEnemy;
    private GameObject enemy;

    private EnemyScript enemyScript;
    private int playerLayer;
    private int boundariesLayer;
    private int groundLayer;
    private int enemyLayer;
    private int doorLayer;
    private int scytheLayer;
    private bool hanging;
    private Animator animator;
    private CinemachineVirtualCamera vcam;
    private Animator playerAnimator;
    private AudioManagerScript audioManager;
    [SerializeField] private GameObject impactParticle;
    [SerializeField] private GameObject impactPoint;
    private GameObject scythe;

    [SerializeField] private float scytheVel = 20f;
    [SerializeField] private float evaVel = 15f;

    void Start() {
        Physics2D.IgnoreLayerCollision(scytheLayer, boundariesLayer);
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        scythe = GameObject.FindGameObjectWithTag("Guadaña");
        playerLayer = LayerMask.NameToLayer("Player");
        boundariesLayer = LayerMask.NameToLayer("Boudaries");
        groundLayer = LayerMask.NameToLayer("Ground");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        scytheLayer = LayerMask.NameToLayer("Guadaña");
        //Si no choca contra nada la guadaña, desaparece tras 0.5 segundos
        Invoke("AutoDestruccion", 0.35f);
        //Cuando spawnea el gancho, el colider del personaje no afecta
        Physics2D.IgnoreLayerCollision(scytheLayer, playerLayer);
        shouldMove = true;
        doorLayer = LayerMask.NameToLayer("Puerta");
        animator = GetComponent<Animator>();
        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        playerAnimator = player.GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManagerScript>();
        activeScytheEnemy = false;
    }

    private void FixedUpdate() {
        //Cuando gancho choca contra pared, eva se desplazaz hacia su posicion, la gravedad deja de afectarle y la aceleracion previa que tenia se anula
        if (activeScythe) {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, evaVel * Time.fixedDeltaTime);
            playerRb.gravityScale = 0;
            playerRb.velocity = Vector3.zero;
            playerAnimator.SetBool("VolandoGancho", true);
        }
        if (activeScytheEnemy) {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, player.transform.position, evaVel * Time.fixedDeltaTime);
            transform.position = enemy.transform.position;
            enemyScript.Stunned = true;
            if (enemyScript.ReachedPlayer) {
                EvaNormalidad();
            }
        }
    }

    private void Update() {
        if(hit) {
            Physics2D.IgnoreLayerCollision(enemyLayer, scytheLayer);
        } else {
            Physics2D.IgnoreLayerCollision(enemyLayer, scytheLayer, false);
        }

        //Movimiento de gancho
        if (shouldMove) {
            transform.Translate(new Vector2(0f, scytheVel) * Time.deltaTime);
        }

        //Cuando eva toca el suelo, puede volver afectarle la relentizacion de lanzar el gancho en el aire y recupera el control
        if (EvaMovement.Instance.IsGrounded) {
            aerialHook = true;
            EvaMovement.Instance.enabled = true;
        }

        if (EvaMovement.Instance.TocandoPared) {
            playerAnimator.SetBool("VolandoGancho", false);
            Destroy(gameObject);
            //Cuando eva llega, la gravedad se pone a esto y luego en EvaMovement, va aumentando hasta que toca suelo y vuelve a 7
            playerRb.gravityScale = 5;
            playerRb.velocity = Vector3.zero;
            activeScythe = false;
            hanging = true;
            SpriteRenderer guadañaRenderer = scythe.GetComponent<SpriteRenderer>();
            guadañaRenderer.enabled = true;
            EvaGanchoScript.Instance.ScytheLight.SetActive(true);
            scythe.transform.position = GameObject.FindGameObjectWithTag("ColgandoHand").transform.position;
            GameManagerScript.Instance.InputEnabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == 8 || collision.gameObject.layer == 14) {
            audioManager.Play("ImpactoGancho");
            activeScythe = true;
            transform.Translate(Vector3.zero);
            //Este invoke hace que cuando choca contra una pared, pueda meterse un poco dentro de la textura haciendo asi que eva pueda acercarse mas a la pared donde ha chocado
            Invoke("StopMovingDelay", .03f);
            hit = true;
            Invoke("AutoDestruccionCuandoBug", 0.5f);
            playerAnimator.SetBool("Gancho", false);
            Physics2D.IgnoreLayerCollision(12, 9, false);
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 8f;
            GameManagerScript.Instance.RecolocarCamara = true;
            Instantiate(impactParticle, impactPoint.transform.position, impactPoint.transform.rotation);
        }

        //En vez de que a cierta distancia del player desaparezca, que cuando el colider choca con el de la guadaña, desaparezca
        //Esto es para cuando eva llega al destino PERO no toca la pared, como por ejemplo a un techo
        if (collision.gameObject == player) {
            EvaNormalidad();
            //Cuando eva llega, la gravedad se pone a esto y luego en EvaMovement, va aumentando hasta que toca suelo y vuelve a 7
            playerRb.velocity = Vector3.zero;
            activeScythe = false;
            hanging = true;
            scythe.transform.position = GameObject.FindGameObjectWithTag("ColgandoHand").transform.position;

        }
        if(collision.gameObject.layer == enemyLayer) {
            GetComponent<BoxCollider2D>().enabled = false;
            playerAnimator.SetBool("VolandoGancho", false);
            playerRb.gravityScale = 7;
            enemy = collision.gameObject;
            enemyScript = enemy.GetComponent<EnemyScript>();
            enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            audioManager.Play("ImpactoGancho");
            Invoke("StopMovingDelay", .03f);
            hit = true;
            activeScytheEnemy = true;
            GameManagerScript.Instance.InputEnabled = true;
            Invoke("AutoDestruccionCuandoBug", 1f);

            if (gameObject.transform.eulerAngles.y < 179 && !enemyScript.FacingRight) {
                enemy.transform.Rotate(0f, 180f, 0f);
                enemyScript.FacingRight = !enemyScript.FacingRight;
            } else if(gameObject.transform.eulerAngles.y > 179 && enemyScript.FacingRight) {
                enemy.transform.Rotate(0f, 180f, 0f);
                enemyScript.FacingRight = !enemyScript.FacingRight;
            }
        }       
    }

    void AutoDestruccion() {
        if (!hit) {
            EvaNormalidad();
        }
    }
    
    void StopMovingDelay() {
        shouldMove = false;
    }

    void AutoDestruccionCuandoBug() {
        EvaNormalidad();
    }

    void EvaNormalidad() {
        Destroy(gameObject);
        playerRb.gravityScale = 7;
        playerAnimator.SetBool("VolandoGancho", false);
        SpriteRenderer guadañaRenderer = scythe.GetComponent<SpriteRenderer>();
        guadañaRenderer.enabled = true;
        EvaGanchoScript.Instance.ScytheLight.SetActive(true);
        GameManagerScript.Instance.InputEnabled = true;
        playerAnimator.SetBool("Gancho", false);
    }

    private void OnDestroy() {
        if (activeScytheEnemy) {
            if (enemyScript.Stunned) {
                enemyScript.DelayStunned = true;
            }
        }
    }
}
