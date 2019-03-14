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
    private float width;

    [HideInInspector] public bool isGrounded;
    private bool jump;
    private bool facingRight;
    public static EvaMovement Instance;

    //Variables fuerzas
    public float jumpForce = 1000f;
    [Range(0, 10f)] [SerializeField] private float velocidad = 4f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 velocity = Vector3.zero;
    LayerMask ground;

    private GameObject guadaña;
    private GameObject colgandoHand;
    private GameObject guadañaPosicionInicial;
    private bool colgado;



    void Start () {
        //Get Componentes
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Layer suelo
        groundLayer = LayerMask.NameToLayer("Ground");
        Instance = this;
        width = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
        ground = 1 << LayerMask.NameToLayer("Ground");
        guadaña = GameObject.FindGameObjectWithTag("Guadaña");
        colgandoHand = GameObject.FindGameObjectWithTag("ColgandoHand");
        guadañaPosicionInicial = GameObject.FindGameObjectWithTag("GuadañaPosicionInicial");
        colgado = true;
        guadaña.transform.position = guadañaPosicionInicial.transform.position;
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

        //Detector de si hay una pared delante de eva 
        Vector2 groundPos = transform.position + transform.right * width;
        Vector2 vec2 = Tovector2(transform.right) * -.02f;
        bool tocandoPared = Physics2D.Linecast(groundPos, groundPos + vec2, ground);
        Debug.DrawLine(groundPos, groundPos + vec2);

        //animacion de colgado en la pared
        if (tocandoPared && !isGrounded && colgado)
        {
            animator.SetBool("Colgando", true); 
            guadaña.transform.position = colgandoHand.transform.position;
            colgado = false;
        }
        //Cuando deja de tocar la pared en el aire deja de hacer la animacion de colgado
        else if (!tocandoPared && !isGrounded && !colgado)
        {
            animator.SetBool("Colgando", false);
            guadaña.transform.position = guadañaPosicionInicial.transform.position;
            colgado = true;
        }
        //Cuando salta hacia arriba pegada a un muro no hace la animacion de deslice hacia arriba
        if(rb.velocity.y > 0) animator.SetBool("Colgando", false);

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
            animator.SetBool("Colgando", false);
            guadaña.transform.position = guadañaPosicionInicial.transform.position;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = false;
        }
    }

    //Metodo para pasar vectores3 a vectores2, bueno para cuando queremos hacer un linecast horizontal para pasar el transform.right a ser vector2
    private Vector3 Tovector2(Vector3 vec3) {
        return new Vector2(vec3.x, vec3.y);
    }
}