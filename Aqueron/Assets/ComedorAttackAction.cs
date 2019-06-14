using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComedorAttackAction : BaseActionComponent
{

    private Rigidbody2D rb;
    private Animator animator;
    private float time;
    [SerializeField] private float attackForce;
    [SerializeField] private float chargeDuration;
    BaseAIComponent.MachineStates finishedState = BaseAIComponent.MachineStates.Attack;
    private GameObject player;

    protected override void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = EvaMovement.Instance.gameObject;
    }

    public override void StartAction() {
        StartCoroutine(Action());
    }

    protected override IEnumerator Action() {
        time = Time.time;
        rb.velocity = Vector2.zero;
        while (Time.time < time + chargeDuration) {
            yield return new WaitForEndOfFrame();
        }
        animator.SetTrigger("Attack");
        if (transform.eulerAngles.y > 179f)
            rb.AddForce(new Vector2(attackForce, 0f));
        else
            rb.AddForce(new Vector2(-attackForce, 0f));

        while (Time.time < time + chargeDuration) {
            yield return new WaitForEndOfFrame();
        }
        ExitAction();
    }

    protected override void ExitAction() {
        if (AIComponent == null)
            AIComponent = GetComponent<BaseAIComponent>() as BaseAIComponent;
        AIComponent.UpdateState(true, finishedState);
    }

    public override void StopAction() {
        StopAllCoroutines();
    }

}
