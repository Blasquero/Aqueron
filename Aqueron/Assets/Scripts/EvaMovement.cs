using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EvaMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator animator;
    private GameObject hangingHand;
    private AudioManagerScript AudioManager;

    #region variables Inspector
    [SerializeField] private GameObject jetpack;
    [SerializeField] private Collider2D circleCollider;
    [SerializeField] private PhysicsMaterial2D normalMaterial;
    [SerializeField] private PhysicsMaterial2D stickyMaterial;
    [SerializeField] private PhysicsMaterial2D superStickyMaterial;
    [SerializeField] private float jumpForce = 1000f;
    [SerializeField] private float jumpForceHorizontalHanging = 400f;
    [SerializeField] private float doubleJumpForce = 700f;
    [Range(0, 10f)] [SerializeField] private float velocity = 4f;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [Range(0, 0.05f)] [SerializeField] private float wallSlipperingVel = 0.015f;
    #endregion

    private float move;
    private int groundLayer;
    private int rampLayer;
    private float width;

    public static EvaMovement Instance;
    #region encapsulación
    //Encapsulación
    private bool isGrounded;
    private bool touchingWall;
    #endregion

    private Vector2 velocityZero = Vector2.zero;
    LayerMask ground;

    private bool hanging;
    private bool jumpAfterHanging;
    private bool alreadyJumpedAfterHanging;
    private bool doubleJump;
    private bool alreadyDoubleJumped;
    private bool airControl;
    private bool slipperIncrease;
    private bool isOnRamp;
    private bool landed;
    private bool alreadyJumped;
    private bool jump;
    private bool facingRight;


    //Para que nose destruya entre escenas y si en estas escenas hay otro como este, el de esa escena se destruye
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void Start () {
        AudioManager = FindObjectOfType<AudioManagerScript>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundLayer = LayerMask.NameToLayer("Ground");
        rampLayer = LayerMask.NameToLayer("Rampas");
        width = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;   
        ground = 1 << LayerMask.NameToLayer("Ground");
        hangingHand = GameObject.FindGameObjectWithTag("ColgandoHand");
        hanging = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        AudioManager = FindObjectOfType<AudioManagerScript>();
    }

    void Update() {
        //Detector de si hay una pared delante de eva 
        Vector2 groundPos = transform.position + transform.right * width;
        Vector2 vec2 = transform.right * -.02f; ;
        touchingWall = Physics2D.Linecast(groundPos, groundPos + vec2, ground);
        Debug.DrawLine(groundPos, groundPos + vec2);

        if (GameManagerScript.Instance.InputEnabled) {
            move = Input.GetAxisRaw("Horizontal");
        } else {
            move = 0;
        }

        if (move == 0 || !isGrounded || animator.GetBool("Jump") == true) {
            AudioManager.Stop("Steps");
        }

        //Switcheo entre animacion de idle y run
        if (move != 0) {
            animator.SetFloat("Speed", 1f);
            circleCollider.sharedMaterial = normalMaterial;
        }

        if (move == 0) {
            animator.SetFloat("Speed", 0f);
            circleCollider.sharedMaterial = superStickyMaterial;
        }

        
        //Input salto
        //El isGrounded es para que cuando esta colgando de la pared, si pulsas 2 veces jump, cuando toque el suelo no vuelva a saltar
        if (Input.GetButtonDown("Jump") && isGrounded && !alreadyJumped) {
            AudioManager.Play("Salto");
            airControl = true;
            jump = true;
            alreadyJumped = true;
        }
        //Double salto
        if (Input.GetButtonDown("Jump") && !isGrounded  && !alreadyDoubleJumped && !animator.GetBool("Colgando")) {
            animator.SetBool("DobleSalto", true);
            doubleJump = true;
            alreadyDoubleJumped = true;
            airControl = true;
            jetpack.SetActive(true);
            AudioManager.Play("CoheteEncendiendose");
            AudioManager.Play("CoheteEncendido");
        }

        //Salto Colgando
         if(Input.GetButtonDown("Jump") && animator.GetBool("Colgando")) {
            AudioManager.Play("Salto");
            jumpAfterHanging = true;
            alreadyJumpedAfterHanging = true;
            airControl = false;
        }

        //Cuando eva esta colgando en la pared despues del gancho
        if (touchingWall && !isGrounded && !hanging && rb.velocity.y < 0) {
            animator.SetBool("Colgando", true);
            rb.gravityScale = 5;
            animator.SetBool("Gancho", false);
            AudioManager.Stop("CoheteEncendido");
            AudioManager.Stop("CoheteEncendiendose");
            AudioManager.Play("DeslizandoseComienzo");
            AudioManager.Play("CaidaSalto");
            Invoke("DeslizandoseContinuo", 0.1f);
            animator.SetBool("Jump", false);
            hanging = true;
            slipperIncrease = true;
            jetpack.SetActive(false);
            alreadyDoubleJumped = false;
        } //Cuando deja de tocar la pared pero sigue en el aire
        else if (!touchingWall && !isGrounded && hanging) {
            animator.SetBool("Colgando", false);
            hanging = false;
            rb.gravityScale = 7;
            slipperIncrease = false;
            AudioManager.Stop("DeslizandoseContinuo");
        } else if (touchingWall && isGrounded && hanging) {
            AudioManager.Stop("DeslizandoseContinuo");
            hanging = false;
        }

        //Cuando salta hacia arriba pegada a un muro no hace la animacion de deslice hacia arriba
        if (rb.velocity.y > 0) {
            animator.SetBool("Colgando", false);
        }
        //Cuando no esta haciendo la animacion, que no suene, para que cuando salta delante de la pared no suene sin hacer animacion
        if(!animator.GetBool("Colgando")) {
            AudioManager.Stop("DeslizandoseComienzo");
            AudioManager.Stop("DeslizandoseContinuo");
        }
        //Para que cuando se quede en un borde de una rampa y salte, no de bugs con la animacion de saltar ya que no resbala del borde y el trigger esta fuera de rango
        //Pongo el animator.bool para que no afecte a deslizanose en pared
        if (!isGrounded && !animator.GetBool("Colgando")) {
            circleCollider.sharedMaterial = normalMaterial;
        }

        //Cuando esta haciendo la animaciendo de deslizandose por la pared, cambia el material
        if (animator.GetBool("Colgando"))
        {
            circleCollider.sharedMaterial = stickyMaterial;
        }

        if (animator.GetBool("VolandoGancho")) {
            AudioManager.Stop("CoheteEncendido");
            AudioManager.Stop("CoheteEncendiendose");
            jetpack.SetActive(false);
        }

        if (!isGrounded) {
            animator.SetBool("Landed", false);
        }
    }

    private void FixedUpdate() {
        //Movimiento horizontal con smoothing y airControl, en caso del salto despues de estar colgado no nos interesa que eva pueda moverse
        //en el aire hasta que pulsa el doble salto otra vez
        if (airControl) {
            Vector3 playerVelocity = new Vector2(move * velocity, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, playerVelocity, ref velocityZero, movementSmoothing);
         }

        //Flipeo del sprite 
        if (move > 0 && facingRight && airControl) {
            Flip();
        } else if (move < 0 && !facingRight && airControl) {
            Flip();
        }

        //Salto y doble salto
        if (jump || doubleJump) {
            if (doubleJump) {
                rb.velocity = Vector3.zero;
            }

            if (jump) { 
                rb.AddForce(new Vector2(0f, jumpForce));
            }

            if (doubleJump) {
                rb.gravityScale = 3;
                rb.AddForce(new Vector2(0f, doubleJumpForce));
            }

            //rb.AddForce(new Vector2(0f, jumpForce));
            animator.SetBool("Jump", true);
            jump = false;
            doubleJump = false;
        }

        //Animacion caida sin que pulsar salto antes
        if (!isGrounded) {
            animator.SetBool("Falling", true);
        } else {
            animator.SetBool("Falling", false);
        }

        //Salto cuando esta colgando de pared que hace que vaya en direccion contraria a esta
        if(jumpAfterHanging && move > 0) {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(-jumpForceHorizontalHanging, jumpForce));
            animator.SetBool("Jump", true);
            jumpAfterHanging = false;
            Flip();
        } else if(jumpAfterHanging && move < 0) {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(jumpForceHorizontalHanging, jumpForce));
            animator.SetBool("Jump", true);
            jumpAfterHanging = false;
            Flip();
        }

        if (slipperIncrease) {
            rb.gravityScale += wallSlipperingVel;
        }
    }

    //Metodo flipeo sprite
    void Flip() {
        facingRight = !facingRight;
        gameObject.transform.Rotate(0f, 180f, 0f);          
    }

    //Deteccion de isGrounded
    private void OnTriggerEnter2D(Collider2D collision) {
       if(collision.gameObject.layer == groundLayer) {
            isGrounded = true;
            //Esto es para cuando se usa el gancho para que eva deje de pegarse a las paredes si no usa el gancho despues de tocar el suelo
            if(collision.gameObject.layer == groundLayer) circleCollider.sharedMaterial = normalMaterial;
            animator.SetBool("Colgando", false);
            animator.SetBool("Jump", false);
            animator.SetBool("DobleSalto", false);
            alreadyJumpedAfterHanging = false;
            alreadyDoubleJumped = false;
            //Recuperamos el control de movimiento siempre que toquemos el suelo
            airControl = true;
            slipperIncrease = false;
            rb.gravityScale = 7;
            jetpack.SetActive(false);
            AudioManager.Stop("CoheteEncendido");
            AudioManager.Stop("CoheteEncendiendose");
            AudioManager.Play("CaidaSalto");
            animator.SetBool("Gancho", false);
            animator.SetBool("Landed", true);
            Invoke("LandedAnim", 0.4f);
            alreadyJumped = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == groundLayer ) {
            isGrounded = false;
        }
        if (collision.gameObject.layer == rampLayer) {
            isOnRamp = false;
            isGrounded = false;
        }
    }

    public void DobleSaltoFinish() {
        animator.SetBool("DobleSalto", false);
    }

    void DelaySalto() {
        rb.AddForce(new Vector2(0f, jumpForce));
    }

    public void DeslizandoseContinuo() {
        AudioManager.Play("DeslizandoseContinuo");
    }

    //Metodo para poner el la animacion de colgado ya que es un loop y solo se va a escuchar el ultimo momento(cuando toca el suelo)
    public void DeslizandoseFinal() {
        AudioManager.Play("DeslizandoseFinal");
    }

    public void StepsSonido() {
        AudioManager.Play("Steps");
    }

    void LandedAnim() {
        animator.SetBool("Landed", false);
    }

    #region metodos encapsulados
    public bool TocandoPared {
        get { return touchingWall; }
    }

    public bool IsGrounded {
        get { return isGrounded; }
    }
    #endregion
}