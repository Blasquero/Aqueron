using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheBassicAttack : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer render;
    [SerializeField]
    private GameObject scythe;
    private BoxCollider2D scytheHitbox;
    private Animator animatorEva, animator;
    private EvaMovement evaMovement;

    private bool show = false;

    [SerializeField]
    private GameObject eva;

    [SerializeField]
    private float timeBetweenAttacks, scytheDamage;

    private bool attackEnabled;
    [SerializeField]
    private float attackDuration;

    public bool AttackEnabled {
        get { return attackEnabled; }
        set { attackEnabled = value; }
    }

    void Start() {
        render = gameObject.GetComponent<SpriteRenderer>();
        render.enabled = show;
        scytheHitbox = gameObject.GetComponent<BoxCollider2D>();
        animatorEva = eva.GetComponent<Animator>();
        animator = gameObject.GetComponent<Animator>();
        evaMovement = eva.GetComponent<EvaMovement>();
        attackEnabled = true;
    }

    void Update() {
        render.enabled = show;
        gameObject.transform.position = eva.transform.position;
        if (Input.GetKeyDown(KeyCode.G) && attackEnabled) {
            if (evaMovement.isGrounded) {
                attackEnabled = false;
                show = true;
                GameManagerScript.inputEnabled = false;
                animatorEva.SetFloat("Speed", 0.0f);
                Debug.Log("Ataque Iniciado");
                Attack();
            }
        }
    }

    private void Attack() {

        render.enabled = true;
        scytheHitbox.enabled = true;
        scythe.GetComponent<SpriteRenderer>().enabled = false;
        animator.SetBool("Attack", true);
        animatorEva.SetTrigger("Attack");
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            BaseDamageComponent baseDamage = collision.gameObject.GetComponent<BaseDamageComponent>() as BaseDamageComponent;
            if (baseDamage != null) {
                baseDamage.Damage(scytheDamage);
            }
        }
    }

    void ActivateAttack() {
        attackEnabled = true;
    }

    void EndAttack() {
        animator.SetBool("Attack", false);
        GameManagerScript.inputEnabled = true;
        animatorEva.SetBool("Attack", false);
        Debug.Log("Ataque finalizado");
        show = false;
        scythe.GetComponent<SpriteRenderer>().enabled = true;

        scytheHitbox.enabled = false;
        Invoke("ActivateAttack", timeBetweenAttacks);
    }

}
