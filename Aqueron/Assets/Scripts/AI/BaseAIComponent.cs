using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAIComponent : MonoBehaviour
{
    protected MachineStates actualState;

    protected abstract void Start();

    public enum MachineStates {
        Sleep, WakeUp, Attack, Patrolling, MoveToPlayer
    };

    public MachineStates ActualState {
        get { return actualState; }
    }

    public abstract void UpdateState(bool success);
}
