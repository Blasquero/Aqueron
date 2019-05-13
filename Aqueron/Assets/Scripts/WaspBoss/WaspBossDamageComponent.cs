using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspBossDamageComponent : BaseDamageComponent {

    private WaspBossAIComponent aiComponent;

    [SerializeField]
    [Range (0f,1f)] private float damageReductionStingState;

    protected override void Start() {
        base.Start();
        aiComponent = gameObject.GetComponent<WaspBossAIComponent>() as WaspBossAIComponent;
        if (aiComponent == null) {
            Debug.LogError("ERROR: No se ha encontrado componente de IA en jefe avispa");
            Debug.Break();
        }
    }

    public override void Damage(float damage) {
        if (aiComponent.WaspState == WaspBossAIComponent.StateMachine.HidingAttack) {
            base.Damage(1);
        }
        else if (aiComponent.WaspState == WaspBossAIComponent.StateMachine.StingAttack) {
            base.Damage(damage * (1-damageReductionStingState));
        }
        else {
            base.Damage(damage);
        }
    }

    protected override void OnDeath() {
        aiComponent.Death();
    }
}
