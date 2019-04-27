using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvispaScript : MonoBehaviour
{
    private float width;
    LayerMask ground;
    LayerMask puerta;
    private int playerLayer;
    private Rigidbody2D rb;
    private bool isBlockedGround;
    private bool isBlockedPuerta;
    private bool isGrounded;
    private bool movement;
    private bool reachedPlayer;
    //velocidad negativa porque el sprite está mirando hacia la izquierda
    private float velocity = -1f;
    private bool facingRight;
    public static AvispaScript instance;
    void Start()
    {
        //Get Componentes
        width = GetComponent<SpriteRenderer>().bounds.extents.x;
        rb = GetComponent<Rigidbody2D>();
        //Get Layer ground
        ground = 1 << LayerMask.NameToLayer("Ground");
        puerta = 1 << LayerMask.NameToLayer("Puerta");
        instance = this;
        movement = true;
        reachedPlayer = false;
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void FixedUpdate()
    {
        //Linea de longitud .02f vertical
        Vector2 vec2 = transform.right * -.1f;
        //Posicion al extremo horizontal del sprite
        Vector2 groundPos = transform.position + transform.right * -width;
        //Visualizacion de la linea vertical
        Debug.DrawLine(groundPos, groundPos + Vector2.down * 3);
        //Visualizacion de la linea horizontal
        Debug.DrawLine(groundPos, groundPos + vec2);
        //LineCast vertical detecta si el enemigo esta tocando el suelo
        isGrounded = Physics2D.Linecast(groundPos, groundPos + Vector2.down * 3, ground);
        //Linecast horizontal detecta si el enemigo tiene un obstaculo delante
        isBlockedGround = Physics2D.Linecast(groundPos, groundPos + vec2, ground);
        //Linecast horizontal detecta si el enemigo tiene una puerta delante
        isBlockedPuerta = Physics2D.Linecast(groundPos, groundPos + vec2, puerta);

        //Movimiento constanate del enemigo sin aceleración
        if (movement) {
            Vector2 myVel = rb.velocity;
            myVel.x = transform.right.x * velocity;
            rb.velocity = myVel;
        }

        //Flipeo del enemigo en funcion de la deteccion de los linecasts
        if (!isGrounded) {
            Flip();
        }
        if (isBlockedGround || isBlockedPuerta) {
            Flip();
        }
    }

    //Metodo flipeo de sprite
    void Flip() {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == playerLayer) {
            Invoke("MovementBack", 1f);
            reachedPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == playerLayer) {
            reachedPlayer = false;
        }
    }

    #region get-set
    void MovementBack() {
        movement = true;
    }

    public bool Movement {
        get { return movement; }
        set { this.movement = value; }
    }

    public bool ReachedPlayer {
        get { return reachedPlayer;  }
        set { this.reachedPlayer = value; }
    }

    public bool FacingRight {
        get { return facingRight; }
        set { this.facingRight = value; }
    }
    #endregion
}
