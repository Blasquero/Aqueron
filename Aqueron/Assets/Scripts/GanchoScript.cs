using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GanchoScript : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private GameObject player;
    private bool ganchoActivo;
    private bool deberiaMoverse;
    private bool hit;
    private bool ganchoAereo = true;

    private int playerLayer;
    private int boundariesLayer;
    private int groundLayer;
    private int rampaLayer;
    private int guadañaLayer;
    private bool colgando;
    private Animator animator;
    private CinemachineVirtualCamera vcam;
    

    private GameObject guadaña;

    [SerializeField] private float moveSpeedGuadaña = 20f;
    [SerializeField] private float moveSpeedEva = 15f;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(guadañaLayer, boundariesLayer);

        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();

        guadaña = GameObject.FindGameObjectWithTag("Guadaña");

        playerLayer = LayerMask.NameToLayer("Player");
        boundariesLayer = LayerMask.NameToLayer("Boudaries");
        groundLayer = LayerMask.NameToLayer("Ground");
        guadañaLayer = LayerMask.NameToLayer("Guadaña");

        //Si no choca contra nada la guadaña, desaparece tras 0.5 segundos
        Invoke("AutoDestruccion", 0.35f);
        //Cuando spawnea el gancho, el colider del personaje no afecta
        Physics2D.IgnoreLayerCollision(guadañaLayer, playerLayer);
        deberiaMoverse = true;
        rampaLayer = LayerMask.NameToLayer("Rampas");
        animator = GetComponent<Animator>();
        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
    }

    private void FixedUpdate()
    {
        //Cuando gancho choca contra pared, eva se desplazaz hacia su posicion, la gravedad deja de afectarle y la aceleracion previa que tenia se anula
        if (ganchoActivo)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, moveSpeedEva * Time.fixedDeltaTime);
            playerRb.gravityScale = 0;
            playerRb.velocity = Vector3.zero;
            player.GetComponent<Animator>().SetBool("VolandoGancho", true);
            animator.SetTrigger("GanchoActivo");
        }

        //Cuando eva lanza la guadaña, cae lentamente (le pongo el ganchoAereo para que solo le afecte una vez y no todo el rato lo cual haria un descenso raro)
  /*      if (gameObject != null && !EvaMovement.Instance.isGrounded && ganchoAereo)
        {
            playerRb.gravityScale = 4;
            playerRb.velocity = Vector3.zero;
            ganchoAereo = false;
        }*/
    }

    private void Update()
    {       
        //Movimiento de gancho
        if (deberiaMoverse)
            transform.Translate(new Vector2(0f, moveSpeedGuadaña) * Time.deltaTime);

        //Cuando eva toca el suelo, puede volver afectarle la relentizacion de lanzar el gancho en el aire y recupera el control
        if (EvaMovement.Instance.isGrounded)
        {
            ganchoAereo = true;
            EvaMovement.Instance.enabled = true;
        }
        if (EvaMovement.Instance.tocandoPared)
        {
            player.GetComponent<Animator>().SetBool("VolandoGancho", false);
            Destroy(gameObject);
            //Cuando eva llega, la gravedad se pone a esto y luego en EvaMovement, va aumentando hasta que toca suelo y vuelve a 7
            playerRb.gravityScale = 5;
            playerRb.velocity = Vector3.zero;
            ganchoActivo = false;
            colgando = true;
            SpriteRenderer guadañaRenderer = guadaña.GetComponent<SpriteRenderer>();
            guadañaRenderer.enabled = true;
            guadaña.transform.position = GameObject.FindGameObjectWithTag("ColgandoHand").transform.position;
            GameManagerScript.inputEnabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == groundLayer || collision.gameObject.layer == rampaLayer)
        {
            FindObjectOfType<AudioManagerScript>().Play("ImpactoGancho");
            ganchoActivo = true;
            transform.Translate(Vector3.zero);
            //Este invoke hace que cuando choca contra una pared, pueda meterse un poco dentro de la textura haciendo asi que eva pueda acercarse mas a la pared donde ha chocado
            Invoke("StopMovingDelay", .03f);
            hit = true;
            Invoke("AutoDestruccionCuandoBug", 1f);
            player.GetComponent<Animator>().SetBool("Gancho", false);
            Physics2D.IgnoreLayerCollision(12, 9, false);
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 8f;
            GameManagerScript.Instance.recolocarCamara = true;

        }
        //En vez de que a cierta distancia del player desaparezca, que cuando el colider choca con el de la guadaña, desaparezca
           if(collision.gameObject == player)
        {
            player.GetComponent<Animator>().SetBool("VolandoGancho", false);
            Destroy(gameObject);
            //Cuando eva llega, la gravedad se pone a esto y luego en EvaMovement, va aumentando hasta que toca suelo y vuelve a 7
            player.GetComponent<Rigidbody2D>().gravityScale = 7;
            playerRb.velocity = Vector3.zero;
            ganchoActivo = false;
            colgando = true;
            SpriteRenderer guadañaRenderer = guadaña.GetComponent<SpriteRenderer>();
            guadañaRenderer.enabled = true;
            guadaña.transform.position = GameObject.FindGameObjectWithTag("ColgandoHand").transform.position;
            GameManagerScript.inputEnabled = true;
        }
    }

    void AutoDestruccion()
    {
        if (!hit)
        {
            Destroy(gameObject);

            player.GetComponent<Rigidbody2D>().gravityScale = 7;
            SpriteRenderer guadañaRenderer = guadaña.GetComponent<SpriteRenderer>();
            guadañaRenderer.enabled = true;
            GameManagerScript.inputEnabled = true;
            player.GetComponent<Animator>().SetBool("Gancho", false);
        }
    }
    
    void StopMovingDelay()
    {
        deberiaMoverse = false;
    }
    void AutoDestruccionCuandoBug()
    {
        player.GetComponent<Rigidbody2D>().gravityScale = 7;
        player.GetComponent<Animator>().SetBool("VolandoGancho", false);
        Destroy(gameObject);
        player.GetComponent<Rigidbody2D>().gravityScale = 7;
        SpriteRenderer guadañaRenderer = guadaña.GetComponent<SpriteRenderer>();
        guadañaRenderer.enabled = true;
        GameManagerScript.inputEnabled = true;
        player.GetComponent<Animator>().SetBool("Gancho", false);
    }
}
