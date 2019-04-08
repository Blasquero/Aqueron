using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamageComponent : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    protected BaseStatsComponent statsComponent;

    [SerializeField]
    protected float iTime = 1; //Segundos que el personaje es invulnerable tras recibir daño
    protected bool isInmortal;

    private float startTime;

    #region GettersSetters
    public bool IsInmortal {
        get { return isInmortal; }
    }
    #endregion
    
    protected virtual void Start() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        statsComponent = gameObject.GetComponent<BaseStatsComponent>() as BaseStatsComponent;
        if (statsComponent == null) {
            Debug.LogError("ERROR: No se ha detectado componente de Stats en "+gameObject.name);
            Debug.Break();
        }
    }

    public virtual void Damage(float damage) {
        if (!isInmortal && statsComponent!= null) {
            float newHealth = statsComponent.ChangeHealth(-damage);
            if (newHealth == 0) {
                this.OnDeath();
            }
            else {
                isInmortal = true;
                startTime = Time.time;
                StartCoroutine("OnDamage");
            }
        }
    }


    protected virtual IEnumerator OnDamage() {

        while (Time.time < startTime + iTime) {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        spriteRenderer.enabled = true;
        isInmortal = false;
    }
    //Método para el efecto de pulleo en Ataque 4 de Eva
    public virtual void PullEffect() { }

    protected virtual void OnDeath() {
        Destroy(gameObject);
        Debug.Log("Enemigo " + gameObject.name + " muerto");
    }

}