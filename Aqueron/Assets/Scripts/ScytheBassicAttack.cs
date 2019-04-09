using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheBassicAttack : MonoBehaviour {

    [SerializeField]
    private GameObject scythe, eva;
    private SpriteRenderer render, renderScythe;
    private BoxCollider2D scytheHitbox;
    private Animator animatorEva, animator;
    private EvaMovement evaMovement;
    private BaseStatsComponent evaStats;

    [SerializeField]
    private float timeBetweenAttacks, scytheDamage;
    private bool attackEnabled;

    #region GettersSetters
    public bool AttackEnabled {
        get { return attackEnabled; }
        set { attackEnabled = value; }
    }

    public float TimeBetweenAttacks {
        get { return timeBetweenAttacks; }
        set {
            if (value > 0) {
                timeBetweenAttacks = value;
            }
        }
    }
    #endregion

    void Start() {
        render = gameObject.GetComponent<SpriteRenderer>();
        scytheHitbox = gameObject.GetComponent<BoxCollider2D>();
        animatorEva = eva.GetComponent<Animator>();
        animator = gameObject.GetComponent<Animator>();
        evaMovement = eva.GetComponent<EvaMovement>();
        evaStats = eva.GetComponent<BaseStatsComponent>() as BaseStatsComponent;
        if (evaStats == null) {
            Debug.LogError("ERROR: No se ha detectado componente de Stats en Eva");
            Debug.Break();
        }
        renderScythe = scythe.GetComponent<SpriteRenderer>();
        attackEnabled = true;
        render.enabled = false;

    }

    void Update() {
        gameObject.transform.position = eva.transform.position;
        if (Input.GetKeyDown(KeyCode.G) && attackEnabled) {
            if (evaMovement.IsGrounded) {
                attackEnabled = false;
                GameManagerScript.Instance.InputEnabled = false;
                animatorEva.SetFloat("Speed", 0.0f);
                Attack();
            }
        }
    }

    private void Attack() {
        render.enabled = true;
        scytheHitbox.enabled = true;
        renderScythe.enabled = false;
        animator.SetBool("Attack", true);
        animatorEva.SetTrigger("Attack");
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            BaseDamageComponent baseDamage = collision.gameObject.GetComponent<BaseDamageComponent>() as BaseDamageComponent;
            if (baseDamage != null) {
                baseDamage.Damage(evaStats.Damage);
            }
            else {
                Debug.LogError("ERROR: Enemigo " + collision.gameObject.name + " sin componente de daño");
                Debug.Break();
            }
        }
    }

    void ActivateAttack() {
        attackEnabled = true;
    }

    void EndAttack() {
        animator.SetBool("Attack", false);
        GameManagerScript.Instance.InputEnabled = true;
        animatorEva.SetBool("Attack", false);
        Debug.Log("Ataque finalizado");
        renderScythe.enabled = true;
        render.enabled = false;
        scytheHitbox.enabled = false;
        Invoke("ActivateAttack", timeBetweenAttacks);
    }

}
