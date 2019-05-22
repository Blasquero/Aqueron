using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSleepAction : BaseActionComponent {

    [SerializeField] protected GameObject player;
    private float time;
    private float duration;

    protected override void Start() {
        AIComponent = GetComponent<BaseAIComponent>() as BaseAIComponent;
        duration = 3;
    }

    public override void StartAction() {
        StartCoroutine("Action");
        Debug.Log("Empezando Sleep");
    }

    protected override IEnumerator Action() {
        time = Time.time;
        while(Time.time < time + duration) {
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
        AIComponent.UpdateState(true, nextState);
    }

}
