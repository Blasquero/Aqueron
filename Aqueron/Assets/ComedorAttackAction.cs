using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComedorAttackAction : BaseActionComponent
{

    private Rigidbody2D rb;
    private Animator animator;

    protected override void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public override void StartAction() {
        StartCoroutine(Action());
    }

    protected override IEnumerator Action() {
        animator.SetTrigger("Attack");
        rb.velocity = Vector2.zero;
        if (transform.eulerAngles.y > 179f)
            rb.AddForce(new Vector2(400f, 0f));
        else
            rb.AddForce(new Vector2(-400f, 0f));
        yield return new WaitForEndOfFrame();
        ExitAction();
    }

    protected override void ExitAction() {
        if (AIComponent == null)
            AIComponent = GetComponent<BaseAIComponent>() as BaseAIComponent;
        AIComponent.UpdateState(true);
    }

}
