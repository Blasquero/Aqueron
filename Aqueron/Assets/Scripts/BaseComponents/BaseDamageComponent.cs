using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamageComponent : MonoBehaviour {
    [SerializeField]
    private float iTime = 1;
    private bool inmortal;

    private float startTime;

    public virtual void Damage(float damage) {
        if (!inmortal) {
            float newHealth = gameObject.GetComponent<BaseStatsComponent>().ChangeHealth(-damage);
            if (newHealth == 0) {
                this.OnDeath();
            }
            else {
                inmortal = true;
                startTime = Time.time;
                StartCoroutine("OnDamage");
            }
        }

    }


    public virtual IEnumerator OnDamage() {

        while (Time.time < startTime + iTime) {
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        inmortal = false;
    }
    //Método para el efecto de pulleo en Ataque 4 de Eva
    public virtual void PullEffect() { }

    public virtual void OnDeath() {
        Destroy(gameObject);
        Debug.Log("Enemigo " + gameObject.name + " muerto");
    }

}