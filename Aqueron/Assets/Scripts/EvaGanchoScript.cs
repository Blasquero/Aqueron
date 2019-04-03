﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EvaGanchoScript : MonoBehaviour
{
    public GameObject gancho;
    public Transform ganchoTrans;
    private bool attackDone;
    private GameObject guadaña;
    private Animator animator;
    private Rigidbody2D rb;

    public GameObject startPointChain;
    LineRenderer chain;
    GameObject GanchoClone;
    private void Start()
    {
        guadaña = GameObject.FindGameObjectWithTag("Guadaña");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        chain = gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (animator.GetBool("Falling") == true) FindObjectOfType<AudioManagerScript>().Stop("Steps");
        if (Input.GetButtonDown("Vertical") && !attackDone)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Gancho", true);
            animator.SetBool("Jump", false);
            Invoke("LanzarGancho", 0.20f);
            //Cuando lanzas el gancho, eva no puede moverse
            if (!EvaMovement.Instance.isGrounded)
            {
                GameManagerScript.inputEnabled = false;
                rb.gravityScale = 0f;
                rb.velocity = Vector3.zero;
            }
            attackDone = true;
        }

        
        Vector3 chainStartPos = startPointChain.transform.position;
        chain.SetPosition(0, chainStartPos);
        Vector3 chainEndPos;
        if (GanchoClone != null)
        {
            chainEndPos = GanchoClone.transform.position;
            
        }
        else
        {
            chainEndPos = chainStartPos;

        }
        chain.SetPosition(1, chainEndPos);
    }
    
    //Metodo para usar en invoke para cooldown entre ganchos
    void AttackDone() {
        attackDone = false;
      
    }

    void LanzarGancho()
    {
        FindObjectOfType<AudioManagerScript>().Play("LanzarGancho");
        //Hace aparecer el gancho
        GanchoClone = Instantiate(gancho, ganchoTrans.position, ganchoTrans.rotation);
        
       
        

        //Cooldown gancho
        Invoke("AttackDone", 1f);

        //Guadaña detras de eva desaparece mientras se mueve en forma d egancho
        SpriteRenderer guadañaRenderer = guadaña.GetComponent<SpriteRenderer>();
        guadañaRenderer.enabled = false;

    }

}
