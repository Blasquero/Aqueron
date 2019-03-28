using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamageComponent : MonoBehaviour {
    [SerializeField]
    private float iTime=1;

    [SerializeField]
    private bool activateTest;

    private float startTime;

    private void Testing() {
        if (activateTest) {
            activateTest = false;
            Damage(0);
        }
    }

    public virtual void Damage(float damage) {
        float newHealth = gameObject.GetComponent<BaseStatsComponent>().ChangeHealth(-damage);
        if (newHealth== 0) {
           this.OnDeath();
        }
        else {
            startTime = Time.time;
            StartCoroutine("OnDamage");
        }
    }


    public virtual IEnumerator OnDamage() {
        while (Time.time < startTime + startTime) {
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.1f);
        }
    }
    //Método para el efecto de pulleo en Ataque 4 de Eva
    public virtual void PullEffect() { }

    public virtual void OnDeath() {
        Destroy(gameObject);
        Debug.Log("Enemigo " + gameObject.name + " muerto");
    }

}