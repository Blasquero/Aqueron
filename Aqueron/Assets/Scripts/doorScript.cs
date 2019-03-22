using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    private bool locked;
    private bool sensor;

    private Animator animator;


    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if(gameObject.tag == "UnlockedDoor") animator.SetBool("locked", false);       
        if(gameObject.tag == "LockedDoor") animator.SetBool("locked", true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
            animator.SetBool("sensor", true);

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
            animator.SetBool("sensor", false);
    }

}
