using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaMovement : MonoBehaviour {

    //Variables componentes
    private Rigidbody2D rb;
    private Animator animator;
    public Collider2D circleCollider;
    public PhysicsMaterial2D normalMaterial;

    private float move;
    private int groundLayer;

    [HideInInspector] public bool isGrounded;
    private bool jump;
    private bool facingRight;
    public static EvaMovement Instance;

    //Variables fuerzas
    public float jumpForce = 1000f;
    [Range(0, 10f)] [SerializeField] private float velocidad = 4f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 velocity = Vector3.zero;




    void Start () {
        //Get Componentes
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Layer suelo
        groundLayer = LayerMask.NameToLayer("Ground");
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        //Input movimiento horizontal
        move = Input.GetAxisRaw("Horizontal");
        //Switcheo entre animacion de idle y run
        if (move != 0) animator.SetFloat("Speed", 1f);
        if (move == 0) animator.SetFloat("Speed", 0f);
        //Input salto
        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
       
	}

    private void FixedUpdate()
    {
        //Movimiento horizontal con smoothing
        Vector3 playerVelocity = new Vector2(move * velocidad, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, playerVelocity, ref velocity, m_MovementSmoothing);

        //Flipeo del sprite 
        if (move > 0 && facingRight) Flip();
        else if (move < 0 && !facingRight) Flip();

        //Salto 
        if (jump == true && isGrounded == true)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            animator.SetBool("Jump", true);
            jump = false;
        } else if (jump == false && isGrounded == true)
        {
            animator.SetBool("Jump", false);
        }

        if (isGrounded == false) animator.SetBool("Falling", true);
        else animator.SetBool("Falling", false);
            
    }

    //Metodo flipeo sprite
    void Flip()
    {
        facingRight = !facingRight;
        gameObject.transform.Rotate(0f, 180f, 0f);
           
    }

    //Deteccion de isGrounded
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.layer == groundLayer)
        {
            isGrounded = true;
            //Esto es para cuando se usa el gancho para que eva deje de pegarse a las paredes si no usa el gancho despues de tocar el suelo
            circleCollider.sharedMaterial = normalMaterial;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = false;
        }
    }
}