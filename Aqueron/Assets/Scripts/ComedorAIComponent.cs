using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComedorAIComponent : BaseAIComponent {

    private BaseActionComponent comedorPatrollingAction;
    private BaseActionComponent comedorMoveToPlayer;
    private BaseActionComponent comedorattackAction;
    private GameObject player;
    private int numAttacks;

    protected override void Start() {
        comedorPatrollingAction = GetComponent<ComedorPatrollingAction>() as BaseActionComponent;
        comedorMoveToPlayer = GetComponent<ComedorMoveToPlayer>() as BaseActionComponent;
        comedorattackAction = GetComponent<ComedorAttackAction>() as BaseActionComponent;
        player = EvaMovement.Instance.gameObject;
        actualState = MachineStates.Patrolling;
        comedorPatrollingAction.StartAction();
    }

    public override void UpdateState(bool success, MachineStates finishedState) {
        if (Vector2.Distance(transform.position, player.transform.position) > 5) {
            comedorPatrollingAction.StartAction();
            actualState = MachineStates.Patrolling;
        } else {
            if (actualState == MachineStates.MoveToPlayer && finishedState == MachineStates.MoveToPlayer && success) {
                comedorattackAction.StartAction();
                actualState = MachineStates.Attack;
            }
        }
        if (actualState == MachineStates.Attack && finishedState == MachineStates.Attack && success) {
            comedorMoveToPlayer.StartAction();
            actualState = MachineStates.MoveToPlayer;
        }
    }

    public void Update() {
        if (actualState == MachineStates.Patrolling && Vector2.Distance(transform.position, player.transform.position) < 5) {
            comedorPatrollingAction.StopAction();
            actualState = MachineStates.MoveToPlayer;
            comedorMoveToPlayer.StartAction();
        }
    }
}
