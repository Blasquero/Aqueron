using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComedorPatrollingAction : BaseActionComponent
{

    [SerializeField] protected Transform leftPoint;
    [SerializeField] protected Transform rightPoint;
    [SerializeField] protected float speed;
    BaseAIComponent.MachineStates finishedState = BaseAIComponent.MachineStates.Patrolling;

    protected override void Start() {
        AIComponent = GetComponent<ComedorAIComponent>() as BaseAIComponent;
        speed = 2f;

    }

    public override void StartAction() {
        StartCoroutine(Action());
    }

    protected override IEnumerator Action() {
        while (Vector2.Distance(transform.position, rightPoint.position) > 0.1) {
            transform.position = Vector2.MoveTowards(transform.position, rightPoint.position, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        while (Vector2.Distance(transform.position, leftPoint.position) > 0.1) {
            transform.position = Vector2.MoveTowards(transform.position, leftPoint.position, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        ExitAction();
    }

    protected override void ExitAction( ) {
        if(AIComponent == null)
            AIComponent = GetComponent<BaseAIComponent>() as BaseAIComponent;
        AIComponent.UpdateState(true, finishedState);
    }

    public override void StopAction() {
        StopAllCoroutines();
    }
}
