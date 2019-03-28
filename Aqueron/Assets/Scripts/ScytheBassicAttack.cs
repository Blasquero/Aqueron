using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheBassicAttack : MonoBehaviour {

    private BoxCollider2D   scytheHitbox;
    private Animator animator;
    private EvaMovement evaMovement;
    public bool activo;

    [SerializeField]
    private GameObject eva;

    [SerializeField]
    private float timeBetweenAttacks, scytheDamage;

    private bool attackEnabled;

    public bool AttackEnabled {
        get { return attackEnabled; }
        set { attackEnabled = value; }
    }

    void Start() {
        scytheHitbox = gameObject.GetComponent<BoxCollider2D>();
        animator = eva.GetComponent<Animator>();
        evaMovement = eva.GetComponent<EvaMovement>();
        attackEnabled = true;
    }

    void Update() {
        activo= GameManagerScript.inputEnabled;
        if(Input.GetKeyDown(KeyCode.G) && attackEnabled) {
            if (evaMovement.isGrounded) {
                GameManagerScript.inputEnabled = false;
                animator.SetFloat("Speed", 0.0f);

                Debug.Log("Ataque Iniciado");
                attackEnabled = false;
                Attack2();
            }
        }
    }

    private void Attack2() {
        animator.SetBool("Attack",true);
        scytheHitbox.isTrigger = true;
        StartCoroutine("CheckEndAttack");
    }

    private IEnumerator CheckEndAttack() {
        if (animator.GetBool("Attack")){
            yield return new WaitForEndOfFrame();
        }
        EndAttack();
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            scytheDamage=eva.GetComponent<BaseStatsComponent>().Damage;
            collision.gameObject.GetComponent<BaseDamageComponent>().Damage(scytheDamage);
        }
    }
  
    void ActivateAttack() {
        attackEnabled = true;
    }

    void EndAttack() {
        animator.SetBool("Attack", false);
        Debug.Log("Ataque finalizado");
        scytheHitbox.isTrigger = false;
        GameManagerScript.inputEnabled = true;
        Invoke("ActivateAttack", timeBetweenAttacks);
    }

    void Attack() {
        scytheHitbox.isTrigger = true;
        animator.SetBool("Attack", true);
    }
}
