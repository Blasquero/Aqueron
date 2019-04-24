using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GanchoScript : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private GameObject player;
    private bool activeScythe;
    private bool shouldMove;
    private bool hit;
    private bool aerialHook = true;

    private int playerLayer;
    private int boundariesLayer;
    private int groundLayer;
    private int rampLayer;
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
        scytheLayer = LayerMask.NameToLayer("Guadaña");
        //Si no choca contra nada la guadaña, desaparece tras 0.5 segundos
        Invoke("AutoDestruccion", 0.35f);
        //Cuando spawnea el gancho, el colider del personaje no afecta
        Physics2D.IgnoreLayerCollision(scytheLayer, playerLayer);
        shouldMove = true;
        rampLayer = LayerMask.NameToLayer("Rampas");
        animator = GetComponent<Animator>();
        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        playerAnimator = player.GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManagerScript>();
    }

    private void FixedUpdate() {
        //Cuando gancho choca contra pared, eva se desplazaz hacia su posicion, la gravedad deja de afectarle y la aceleracion previa que tenia se anula
        if (activeScythe) {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, evaVel * Time.fixedDeltaTime);
            playerRb.gravityScale = 0;
            playerRb.velocity = Vector3.zero;
            playerAnimator.SetBool("VolandoGancho", true);
            animator.SetTrigger("GanchoActivo");
        }
    }

    private void Update() {
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
            scythe.transform.position = GameObject.FindGameObjectWithTag("ColgandoHand").transform.position;
            GameManagerScript.Instance.InputEnabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == groundLayer || collision.gameObject.layer == rampLayer) {
            audioManager.Play("ImpactoGancho");
            activeScythe = true;
            transform.Translate(Vector3.zero);
            //Este invoke hace que cuando choca contra una pared, pueda meterse un poco dentro de la textura haciendo asi que eva pueda acercarse mas a la pared donde ha chocado
            Invoke("StopMovingDelay", .03f);
            hit = true;
            Invoke("AutoDestruccionCuandoBug", 1f);
            playerAnimator.SetBool("Gancho", false);
            Physics2D.IgnoreLayerCollision(12, 9, false);
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 8f;
            GameManagerScript.Instance.RecolocarCamara = true;
            Instantiate(impactParticle, impactPoint.transform.position, impactPoint.transform.rotation);
        }

        //En vez de que a cierta distancia del player desaparezca, que cuando el colider choca con el de la guadaña, desaparezca
           if(collision.gameObject == player) {
            playerAnimator.SetBool("VolandoGancho", false);
            Destroy(gameObject);
            //Cuando eva llega, la gravedad se pone a esto y luego en EvaMovement, va aumentando hasta que toca suelo y vuelve a 7
            playerRb.gravityScale = 7;
            playerRb.velocity = Vector3.zero;
            activeScythe = false;
            hanging = true;
            SpriteRenderer guadañaRenderer = scythe.GetComponent<SpriteRenderer>();
            guadañaRenderer.enabled = true;
            scythe.transform.position = GameObject.FindGameObjectWithTag("ColgandoHand").transform.position;
            GameManagerScript.Instance.InputEnabled = true;
        }
    }

    void AutoDestruccion() {
        if (!hit) {
            Destroy(gameObject);
            playerRb.gravityScale = 7;
            SpriteRenderer guadañaRenderer = scythe.GetComponent<SpriteRenderer>();
            guadañaRenderer.enabled = true;
            GameManagerScript.Instance.InputEnabled = true;
            playerAnimator.SetBool("Gancho", false);
        }
    }
    
    void StopMovingDelay() {
        shouldMove = false;
    }

    void AutoDestruccionCuandoBug() {
        playerRb.gravityScale = 7;
        playerAnimator.SetBool("VolandoGancho", false);
        Destroy(gameObject);
        playerRb.gravityScale = 7;
        SpriteRenderer guadañaRenderer = scythe.GetComponent<SpriteRenderer>();
        guadañaRenderer.enabled = true;
        GameManagerScript.Instance.InputEnabled = true;
        playerAnimator.SetBool("Gancho", false);
    }
}
