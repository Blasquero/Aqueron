using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatsComponent : MonoBehaviour {

    [SerializeField]
    protected float maxHealth=0,health=0, maxDamage=0,damage=0,maxSpeed=0, speed=0;


    protected virtual void  Start() {
        if (health*maxHealth*damage*maxDamage*speed*maxSpeed==0) {
            Debug.LogWarning("Warning: Stats con valor nulo en " + gameObject.name);
        }
    }

    #region GettersSetters
    public virtual float Health {
        get { return health; }
        set {
            health = value;
            if (health > maxHealth) {
                health = maxHealth;
            }
            else if (health < 0) {
                health = 0;
            }
        }
    }
    public virtual float PercentageHealth {
        get {
            return (health / maxHealth) * 100; 
        }
    }

    public virtual float Damage {
        get { return damage; }
        set {
            damage = value;
            if (damage > maxDamage) {
                damage = maxDamage;
            }
            else if (damage < 0) {
                damage = 0;
            }
        }
    }

    public virtual float Speed {
        get { return speed; }
        set {
            speed = value;
            if (speed > maxSpeed) {
                speed = maxSpeed;
            }
            else if (speed < 0) {
                speed = 0;
            }
        }
    }
    
    public virtual float ChangeHealth(float variation) {
        health += variation;
        if (health < 0) {
            health = 0;
        }
        else if (health > maxHealth) {
            health = maxHealth;
        }
        return health;
    }

    public virtual float ChangeSpeed(float variation) {
        speed += variation;
        if (speed < 0) {
            speed = 0;
        }
        else if (speed > maxSpeed) {
            speed = maxSpeed;
        }
        return speed;
    }

    public virtual float ChangeDamage(float variation) {
        damage += variation;
        if (damage <= 0) {
            damage = 0;
        }
        else if (damage > maxDamage) {
            damage = maxDamage;
        }
        return damage;
    }
    #endregion

}
