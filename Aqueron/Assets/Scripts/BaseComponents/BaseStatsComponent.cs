using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatsComponent : MonoBehaviour {
    
    //Cualquier stat que un enemigo pueda necesitar debe estar declarada aquí
    [SerializeField]
    private float maxHealt=0,health=0, maxDamage=0,damage=0,maxSpeed=0, speed=0;


    protected virtual void  Start() {
        if (health*maxHealt*damage*maxDamage*speed*maxSpeed==0) {
            Debug.LogWarning("Warning: Stats con valor nulo en " + gameObject.name);
        }
    }

    #region Getters-Setters
    public virtual float Health {
        get { return this.health; }
        set { this.health = value; }
    }

    public virtual float Damage {
        get { return this.damage; }
        set { this.damage = value; }
    }

    public virtual float Speed {
        get { return this.speed; }
        set { this.speed = value; }
    }
    
    public virtual float ChangeHealth(float variation) {
        this.health += variation;
        if (this.health < 0) {
            this.health = 0;
        }
        return this.health;
    }

    public virtual float ChangeSpeed(float variation) {
        this.speed += variation;
        if (this.speed < 0) {
            this.speed = 0;
        }
        return this.speed;
    }

    public virtual float ChangeDamage(float variation) {
        this.damage += variation;
        if (this.damage <= 0) {
            this.damage = 0;
        }
        return this.damage;
    }

    #endregion

}
