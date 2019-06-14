using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ComedorMoveToPlayer : BaseActionComponent
{
    private GameObject player;
    [SerializeField] private float speed;
    private float nextWayPointDistance = 0.5f;
    private int currentWayPoint = 0;
    private bool reachEndOfPath = false;
    private Path path;
    private Seeker seeker;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask playerLayer;
    private bool reachedEva;
    BaseAIComponent.MachineStates finishedState = BaseAIComponent.MachineStates.MoveToPlayer;


    protected override void Start() {
        player = EvaMovement.Instance.gameObject;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void StartAction() {
        StartCoroutine(Action());
    }

    protected override IEnumerator Action() {
        while (!reachedEva) {
            if(seeker.IsDone())
              seeker.StartPath(rb.position, player.transform.position, OnPathCompplete);
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

    private void OnPathCompplete(Path p) {
        if(!p.error) {
            path = p;
        }
    }

    private void FixedUpdate() {
        reachedEva = Physics2D.Linecast(transform.position, (Vector2)transform.right * -1.5f + (Vector2)transform.position, playerLayer);
        Debug.DrawLine(transform.position, (Vector2)transform.right * -1.5f + (Vector2)transform.position);

        if (path == null)
            return;
        if(currentWayPoint >= path.vectorPath.Count) {
            reachEndOfPath = true;
            return;
        } else {
            reachEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;
        rb.AddForce(force);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if(distance < nextWayPointDistance) {
            currentWayPoint++;
        }

        if(rb.velocity.x >= 0.01f) {
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        } else if(rb.velocity.x <= 0.01f) {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }
}
