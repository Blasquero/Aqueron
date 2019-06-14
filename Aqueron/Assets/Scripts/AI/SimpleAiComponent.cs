using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimpleAiComponent : BaseAIComponent {

    private BaseActionComponent patrollingAction;
    private BaseActionComponent sleepAction;
    private int nextState;

    protected override void Start() {
        patrollingAction = GetComponent<BasePatrollingAction>() as BaseActionComponent;
        sleepAction = GetComponent<BaseSleepAction>() as BaseActionComponent;
        StartCoroutine("Intro");
       
    }

    public override void UpdateState(bool success, MachineStates thisState) {
        actualState = thisState;
        switch (nextState) {
            case 1:
                patrollingAction.StartAction();
                thisState = MachineStates.Patrolling;
                break;
            case 2:
                sleepAction.StartAction();
                thisState = MachineStates.Sleep;
                break;
        }
    }

    IEnumerator Intro() {
        Debug.Log("Intro");
        float time = Time.time;
        while(Time.time < time + 3) {
            yield return new WaitForEndOfFrame();
        }
        actualState = MachineStates.Sleep;
        UpdateState(true, MachineStates.Sleep);
    }
}
