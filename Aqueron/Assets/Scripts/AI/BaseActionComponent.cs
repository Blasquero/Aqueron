using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseActionComponent : MonoBehaviour
{
    protected int nextState;
    protected BaseAIComponent AIComponent;

    protected abstract void Start();

    public abstract void StartAction();

    public abstract void StopAction();

    protected abstract IEnumerator Action();

    protected abstract void ExitAction();
}
