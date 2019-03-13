using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int guadañaLayer;

    

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
        //Tras 0.5s, si que afecta el colider del personaje asi cuando llega hasta el, al haber chocado contra algo, triggea que desaparezca
        Invoke("StopIgnoringPlayer", 0.5f);
        deberiaMoverse = true;
    }

    private void FixedUpdate()
    {
        //Cuando gancho choca contra pared, eva se desplazaz hacia su posicion, la gravedad deja de afectarle y la aceleracion previa que tenia se anula
        if (ganchoActivo)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, moveSpeedEva * Time.fixedDeltaTime);
            playerRb.gravityScale = 0;
            playerRb.velocity = Vector3.zero;

        }

        //Cuando eva lanza la guadaña, cae lentamente (le pongo el ganchoAereo para que solo le afecte una vez y no todo el rato lo cual haria un descenso raro)
        if (gameObject != null && !EvaMovement.Instance.isGrounded && ganchoAereo)
        {
            playerRb.gravityScale = 4;
            playerRb.velocity = Vector3.zero;
            ganchoAereo = false;
        }
    }

    private void Update()
    {
        //Movimiento de gancho
        if(deberiaMoverse)
            transform.Translate(new Vector2(0f, moveSpeedGuadaña) * Time.deltaTime);

        //Cuando eva toca el suelo, puede volver afectarle la relentizacion de lanzar el gancho en el aire y recupera el control
        if (EvaMovement.Instance.isGrounded)
        {
            ganchoAereo = true;
            EvaMovement.Instance.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == groundLayer)
        {
            ganchoActivo = true;
            transform.Translate(Vector3.zero);
            //Este invoke hace que cuando choca contra una pared, pueda meterse un poco dentro de la textura haciendo asi que eva pueda acercarse mas a la pared donde ha chocado
            Invoke("StopMovingDelay", .03f);
            hit = true;
        }
        //En vez de que a cierta distancia del player desaparezca, que cuando el colider choca con el de la guadaña, desaparezca
        if(collision.gameObject == player)
        {
            Destroy(gameObject);
            playerRb.gravityScale = 7;
            playerRb.velocity = Vector3.zero;
            ganchoActivo = false;
            SpriteRenderer guadañaRenderer = guadaña.GetComponent<SpriteRenderer>();
            guadañaRenderer.enabled = true;
            EvaMovement.Instance.enabled = true;
        }
    }

    void AutoDestruccion()
    {
        if(!hit)
        Destroy(gameObject);

        player.GetComponent<Rigidbody2D>().gravityScale = 7;
        SpriteRenderer guadañaRenderer = guadaña.GetComponent<SpriteRenderer>();
        guadañaRenderer.enabled = true;
        EvaMovement.Instance.enabled = true;
    }
    
    void StopMovingDelay()
    {
        deberiaMoverse = false;
    }

    void StopIgnoringPlayer()
    {
        Physics2D.IgnoreLayerCollision(12, 9, false);
    }
}
