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

    protected abstract IEnumerator Action();

    protected abstract void ExitAction();
}
