using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private float move;
    private bool facingRight;
    private int groundLayer;
    private bool isGrounded;
    private bool jump;
    public float jumpForce = 1000f;
    [Range(0, 10f)] [SerializeField] private float velocidad = 4f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 velocity = Vector3.zero;
    private Animator animator;
    // Use this for initialization  
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundLayer = LayerMask.NameToLayer("Ground");
	}
	
	// Update is called once per frame
	void Update () {
        move = Input.GetAxisRaw("Horizontal");
        if (move != 0) animator.SetFloat("Speed", 1f);
        if (move == 0) animator.SetFloat("Speed", 0f);
        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
       
	}

    private void FixedUpdate()
    {
        Vector3 playerVelocity = new Vector2(move * velocidad, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, playerVelocity, ref velocity, m_MovementSmoothing);

        if (move > 0 && facingRight) Flip();
        else if (move < 0 && !facingRight) Flip();

        if (jump == true && isGrounded == true)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            animator.SetBool("Jump", true);
            jump = false;
        }
 
            
    }

    void Flip()
    {
        facingRight = !facingRight;
        gameObject.transform.Rotate(0f, 180f, 0f);
           
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.layer == groundLayer)
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
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