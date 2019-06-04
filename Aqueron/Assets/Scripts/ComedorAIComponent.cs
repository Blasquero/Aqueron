using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComedorAIComponent : BaseAIComponent
{

    private BaseActionComponent comedorPatrollingAction;
    private BaseActionComponent comedorMoveToPlayer;
    private BaseActionComponent comedorattackAction;
    private GameObject player;

    protected override void Start() {
        comedorPatrollingAction = GetComponent<ComedorPatrollingAction>() as BaseActionComponent;
        comedorMoveToPlayer = GetComponent<ComedorMoveToPlayer>() as BaseActionComponent;
        comedorattackAction = GetComponent<ComedorAttackAction>() as BaseActionComponent;
        player = EvaMovement.Instance.gameObject;
        actualState = MachineStates.Sleep;
        UpdateState(true);
    }

    public override void UpdateState(bool success) {
        if(Vector2.Distance(transform.position, player.transform.position) > 5) { 
                comedorPatrollingAction.StartAction();
                actualState = MachineStates.Patrolling;              
        } else {
            if(actualState == MachineStates.MoveToPlayer && success) {
                comedorattackAction.StartAction();
                actualState = MachineStates.Attack;
            }
        }
    }

    public void Update() {
        if(actualState == MachineStates.Patrolling && Vector2.Distance(transform.position, player.transform.position) < 5) {
            comedorMoveToPlayer.StartAction();
            actualState = MachineStates.MoveToPlayer;
        }
    }
}
