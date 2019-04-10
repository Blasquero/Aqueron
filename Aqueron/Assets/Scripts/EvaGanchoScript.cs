using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EvaGanchoScript : MonoBehaviour
{
    [SerializeField] private GameObject hook;
    [SerializeField] private Transform hookTrans;
    [SerializeField] private GameObject startPointChain;

    private bool attackDone;
    private GameObject scythe;
    private GameObject scytheClone;
    private Animator animator;
    private Rigidbody2D rb;
    private LineRenderer chain;
    private AudioManagerScript audioManager;

    private void Start() {
        scythe = GameObject.FindGameObjectWithTag("Guadaña");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        chain = gameObject.GetComponent<LineRenderer>();
        audioManager = FindObjectOfType<AudioManagerScript>();
    }

    void Update() {
        if (animator.GetBool("Falling") == true) {
            audioManager.Stop("Steps");
        }

        if (Input.GetButtonDown("Vertical") && !attackDone) {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Gancho", true);
            animator.SetBool("Jump", false);
            Invoke("LanzarGancho", 0.20f);
            //Cuando lanzas el gancho, eva no puede moverse
            if (!EvaMovement.Instance.IsGrounded) {
                GameManagerScript.Instance.InputEnabled = false;
                rb.gravityScale = 0f;
                rb.velocity = Vector3.zero;
            }
            attackDone = true;
        }

        if (!EvaMovement.Instance.IsGrounded)
        {
            startPointChain.transform.localPosition = new Vector3(0.29f,0.62f,0);
        }
        else
        {
            startPointChain.transform.localPosition = new Vector3(0.305f, 0.041f, 0);
        }
        Vector3 chainStartPos = startPointChain.transform.position;
        chain.SetPosition(0, chainStartPos);
        Vector3 chainEndPos;
        
            if (scytheClone != null)
        {
            chainEndPos = GameObject.Find("grip").transform.position;
            
        }
        else
        {
            chainEndPos = chainStartPos;

        }
        chain.SetPosition(1, chainEndPos);
    }
    
    void LanzarGancho() {
        audioManager.Play("LanzarGancho");
        //Hace aparecer el gancho
        scytheClone = Instantiate(hook, hookTrans.position, hookTrans.rotation);
        //Cooldown gancho
        Invoke("AttackDone", 1f);
        //Guadaña detras de eva desaparece mientras se mueve en forma d egancho
        SpriteRenderer guadañaRenderer = scythe.GetComponent<SpriteRenderer>();
        guadañaRenderer.enabled = false;
    }

    //Metodo para usar en invoke para cooldown entre ganchos
    void AttackDone() {
        attackDone = false;
    }
}
