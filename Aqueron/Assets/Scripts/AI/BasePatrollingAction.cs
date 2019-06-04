using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasePatrollingAction : BaseActionComponent {

    [SerializeField] protected Transform rightPoint;
    [SerializeField] protected Transform rightUpPoint;
    [SerializeField] protected Transform LeftPoint;
    [SerializeField] protected GameObject player;

    protected override void Start() {
        AIComponent = GetComponent<BaseAIComponent>() as BaseAIComponent;
    }

    public override void StartAction() {
        StartCoroutine("Action");
        Debug.Log("Empezando Patrolling");
    }

    protected override IEnumerator Action() {
        while (Vector2.Distance(transform.position, rightPoint.position) > 0.1) {
            transform.position = Vector2.MoveTowards(transform.position, rightPoint.position, 4f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        while (Vector2.Distance(transform.position, rightUpPoint.position) > 0.1) {
            transform.position = Vector2.MoveTowards(transform.position, rightUpPoint.position, 10f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        while (Vector2.Distance(transform.position, LeftPoint.position) > 0.1) {
            transform.position = Vector2.MoveTowards(transform.position, LeftPoint.position, 15f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        ExitAction();
    }

    protected override void ExitAction() {
        nextState = GameManagerScript.Instance.Random.Next(1, 3);
        if (transform.position.x - EvaMovement.Instance.transform.position.x > 0) {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        } else {
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        if (AIComponent == null)
            AIComponent = GetComponent<BaseAIComponent>() as BaseAIComponent;
        AIComponent.UpdateState(true);
    }

}
