using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyScript : MonoBehaviour {

    private float width;
    [SerializeField] private LayerMask obstacle;
    LayerMask puerta;
    private int playerLayer;
    private Rigidbody2D rb;
    private bool isBlocked;
    private bool isGrounded;
    private bool movement;
    private Transform playerPos;
    private Transform pos;

    //Stunned while hooked
    private bool stunned;
    //Stunned after hooked
    private bool delayStunned;
    
    private bool reachedPlayer;
    //velocidad negativa porque el sprite está mirando hacia la izquierda
    protected float velocity;
    protected bool facingRight;

    protected virtual void Start() {
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        width = GetComponent<SpriteRenderer>().bounds.extents.x;
        rb = GetComponent<Rigidbody2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        pos = gameObject.transform;
        stunned = false;
        delayStunned = false;   
        reachedPlayer = false;
        velocity = -1f;
    }

    protected virtual void Update() {
        Vector2 vec2 = transform.right * -.1f;
        Vector2 groundPos = transform.position + transform.right * -width;
        Debug.DrawLine(groundPos, groundPos + Vector2.down * 3);
        Debug.DrawLine(groundPos, groundPos + vec2);
        isGrounded = Physics2D.Linecast(groundPos, groundPos + Vector2.down * 3, obstacle);
        isBlocked = Physics2D.Linecast(groundPos, groundPos + vec2, obstacle);

        if (delayStunned) {
            Invoke("MovementBack", 1f);
            Invoke("NoReachedPlayer", 0.1f);
        }

        if (!stunned) {
            Vector2 myVel = rb.velocity;
            myVel.x = transform.right.x * velocity;
            rb.velocity = myVel;

            if (!isGrounded) {
                Flip();
            }
            if (isBlocked) {
                Flip();
            }

        } else {
            if (Vector2.Distance(pos.position, playerPos.position) < 3 && !reachedPlayer) {
                Invoke("MovementBack", 1f);
                reachedPlayer = true;
            }
        }
    }

    //Metodo flipeo de sprite
    void Flip() {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }

    void MovementBack() {
        stunned = false;
        delayStunned = false;
    }

    void NoReachedPlayer() {
        reachedPlayer = false;
    }

    #region get-set

    public bool DelayStunned {
        set { this.delayStunned = value;}
    }

    public bool Stunned {
        get { return stunned; }
        set { this.stunned = value; }
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
