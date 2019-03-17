using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaMovement : MonoBehaviour {

    //Variables componentes
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject llamaSalto;
    public Collider2D circleCollider;
    public PhysicsMaterial2D normalMaterial;
    public PhysicsMaterial2D stickyMaterial;
    public PhysicsMaterial2D superStickyMaterial;

    private float move;
    private int groundLayer;
    private int rampaLayer;
    private float width;

    [HideInInspector] public bool isGrounded;
    private bool jump;
    private bool facingRight;
    public static EvaMovement Instance;

    //Variables fuerzas
    [Range(0, 0.05f)] [SerializeField] private float desliceVelocidad = 0.015f;
    [SerializeField] private float jumpForce = 1000f;
    [Range(0, 10f)] [SerializeField] private float velocidad = 4f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private float jumpForceColgandoHorizontal = 400f;
    private Vector3 velocity = Vector3.zero;
    LayerMask ground;
    public float doubleJumpForce = 700f;

    private GameObject colgandoHand;
    public bool colgado;
    private bool jumpAfterColgado;
    private bool alreadyJumpedAfterColgado;
    private bool doubleJump;
    private bool alreadyDoubleJumped;
    private bool airControl;
    private bool aumentoDeslice;
    private bool isOnRampa;



    void Start () {
        //Get Componentes
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Layer suelo
        groundLayer = LayerMask.NameToLayer("Ground");
        rampaLayer = LayerMask.NameToLayer("Rampas");
        Instance = this;
        width = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
        ground = 1 << LayerMask.NameToLayer("Ground");
        colgandoHand = GameObject.FindGameObjectWithTag("ColgandoHand");
        colgado = false;
      //  guadaña.transform.position = guadañaPosicionInicial.transform.position;
        
	}

    // Update is called once per frame
    void Update() {
        //Detector de si hay una pared delante de eva 
        Vector2 groundPos = transform.position + transform.right * width;
        Vector2 vec2 = Tovector2(transform.right) * -.02f;
        bool tocandoPared = Physics2D.Linecast(groundPos, groundPos + vec2, ground);
        Debug.DrawLine(groundPos, groundPos + vec2);
        //Input movimiento horizontal
        move = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonUp("Horizontal") || !isGrounded) FindObjectOfType<AudioManagerScript>().Stop("Steps");
        //Switcheo entre animacion de idle y run
        if (move != 0) animator.SetFloat("Speed", 1f);
        if (move == 0) animator.SetFloat("Speed", 0f);

        if (move != 0 && isOnRampa) circleCollider.sharedMaterial = normalMaterial;
        if (move == 0 && isOnRampa) circleCollider.sharedMaterial = superStickyMaterial;

        

        //Input salto
        //El isGrounded es para que cuando esta colgando de la pared, si pulsas 2 veces jump, cuando toque el suelo no vuelva a saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            airControl = true;
            jump = true;
        }
        //Double salto
        if (Input.GetButtonDown("Jump") && !isGrounded && !tocandoPared && !alreadyDoubleJumped)
        {
            animator.SetBool("DobleSalto", true);
            doubleJump = true;
            alreadyDoubleJumped = true;
            airControl = true;
            llamaSalto.SetActive(true);
            FindObjectOfType<AudioManagerScript>().Play("CoheteEncendiendose");
            FindObjectOfType<AudioManagerScript>().Play("CoheteEncendido");
        }

        //Activacion de salto de Eva cuando esta colgando en la pared y que solo pueda hacerlo una vez y solo cuando haya hecho 
        //el gancho antes(cuando el material cambie a sticky)
        if (tocandoPared && Input.GetButtonDown("Jump") && !isGrounded && !alreadyJumpedAfterColgado
            && circleCollider.sharedMaterial != normalMaterial)
        {
            jumpAfterColgado = true;
            alreadyJumpedAfterColgado = true;
            airControl = false;
        }

        //Cuando eva esta colgando en la pared despues del gancho
        if (tocandoPared && !isGrounded && !colgado)
        {
            FindObjectOfType<AudioManagerScript>().Stop("CoheteEncendido");
            FindObjectOfType<AudioManagerScript>().Stop("CoheteEncendiendose");
            FindObjectOfType<AudioManagerScript>().Play("DeslizandoseComienzo");
            Invoke("DeslizandoseContinuo", 0.1f);
            animator.SetBool("Colgando", true);
            animator.SetBool("Jump", false);
            colgado = true;
            aumentoDeslice = true;
            llamaSalto.SetActive(false);
        }
        //Cuando deja de tocar la pared pero sigue en el aire
        else if (!tocandoPared && !isGrounded && colgado)
        {
            animator.SetBool("Colgando", false);
            colgado = false;
            rb.gravityScale = 7;
            aumentoDeslice = false;
            FindObjectOfType<AudioManagerScript>().Stop("DeslizandoseContinuo");
        } else if (tocandoPared && isGrounded && colgado)
        {
            FindObjectOfType<AudioManagerScript>().Stop("DeslizandoseContinuo");
            colgado = false;
        } 

        //Cuando salta hacia arriba pegada a un muro no hace la animacion de deslice hacia arriba
        if (rb.velocity.y > 0 ) animator.SetBool("Colgando", false);
        //Cuando no esta haciendo la animacion, que no suene, para que cuando salta delante de la pared no suene sin hacer animacion
        if(!animator.GetBool("Colgando"))
        {          
            FindObjectOfType<AudioManagerScript>().Stop("DeslizandoseComienzo");
            FindObjectOfType<AudioManagerScript>().Stop("DeslizandoseContinuo");
        }
    
    }

    private void FixedUpdate()
    {
        //Movimiento horizontal con smoothing y airControl, en caso del salto despues de estar colgado no nos interesa que eva pueda moverse
        //en el aire hasta que pulsa el doble salto otra vez
        if (airControl) { 
        Vector3 playerVelocity = new Vector2(move * velocidad, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, playerVelocity, ref velocity, m_MovementSmoothing);
         }

        //Flipeo del sprite 
        if (move > 0 && facingRight && airControl) Flip();
        else if (move < 0 && !facingRight && airControl) Flip();

        //Salto y doble salto
        if (jump || doubleJump)
        {
            if (doubleJump) rb.velocity = Vector3.zero;
            if(jump) rb.AddForce(new Vector2(0f, jumpForce));
            if (doubleJump)
            {
                rb.gravityScale = 3;
                rb.AddForce(new Vector2(0f, doubleJumpForce));
            }
            //rb.AddForce(new Vector2(0f, jumpForce));
            animator.SetBool("Jump", true);
            jump = false;
            doubleJump = false;
        } 

        //Animacion caida sin que pulsar salto antes
        if (!isGrounded) animator.SetBool("Falling", true);
        else animator.SetBool("Falling", false);

        //Salto cuando esta colgando de pared que hace que vaya en direccion contraria a esta
        if(jumpAfterColgado && move > 0)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector2(-jumpForceColgandoHorizontal, jumpForce));
            animator.SetBool("Jump", true);
            jumpAfterColgado = false;
            Flip();
        } else if(jumpAfterColgado && move < 0)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector2(jumpForceColgandoHorizontal, jumpForce));
            animator.SetBool("Jump", true);
            jumpAfterColgado = false;
            Flip();
        }

        if (aumentoDeslice) rb.gravityScale += desliceVelocidad;
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
       if(collision.gameObject.layer == groundLayer || collision.gameObject.layer == rampaLayer)
        {
            isGrounded = true;
            //Esto es para cuando se usa el gancho para que eva deje de pegarse a las paredes si no usa el gancho despues de tocar el suelo
            if(collision.gameObject.layer == groundLayer) circleCollider.sharedMaterial = normalMaterial;
            if (collision.gameObject.layer == rampaLayer) isOnRampa = true;
            animator.SetBool("Colgando", false);
            animator.SetBool("Jump", false);
            animator.SetBool("DobleSalto", false);
            alreadyJumpedAfterColgado = false;
            alreadyDoubleJumped = false;
            //Recuperamos el control de movimiento siempre que toquemos el suelo
            airControl = true;
            aumentoDeslice = false;
            rb.gravityScale = 7;
            llamaSalto.SetActive(false);
            FindObjectOfType<AudioManagerScript>().Stop("CoheteEncendido");
            FindObjectOfType<AudioManagerScript>().Stop("CoheteEncendiendose");
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == groundLayer ) isGrounded = false;
        if (collision.gameObject.layer == rampaLayer)
        {
            isOnRampa = false;
            isGrounded = false;
        }
    }

    //Metodo para pasar vectores3 a vectores2, bueno para cuando queremos hacer un linecast horizontal para pasar el transform.right a ser vector2
    private Vector3 Tovector2(Vector3 vec3) {
        return new Vector2(vec3.x, vec3.y);
    }

    public void DobleSaltoFinish()
    {
        animator.SetBool("DobleSalto", false);
    }

    void DelaySalto()
    {
        rb.AddForce(new Vector2(0f, jumpForce));
    }

    public void DeslizandoseContinuo()
    {
        FindObjectOfType<AudioManagerScript>().Play("DeslizandoseContinuo");
    }

    //Metodo para poner el la animacion de colgado ya que es un loop y solo se va a escuchar el ultimo momento(cuando toca el suelo)
    public void DeslizandoseFinal()
    {
        FindObjectOfType<AudioManagerScript>().Play("DeslizandoseFinal");
    }
    public void StepsSonido()
    {
        FindObjectOfType<AudioManagerScript>().Play("Steps");
    }
}