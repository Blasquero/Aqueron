using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaScytheAttack : MonoBehaviour {

    [SerializeField] private BaseStatsComponent evaStats;
    private Animator evaAnimator;
    private GameObject damageBox;

    [SerializeField] private int comboCount;
    [SerializeField] private int maxCombo;
    [SerializeField] private float timeBetweenCombos;
    [SerializeField] private float maxTimeBetweenAttacks;
    [SerializeField] private bool canAttack;

    private float lastAttack;

    #region Getters-Setters
    public int ComboCount {
        get {
            return comboCount;
        }
    }

    public bool CanAttack {
        get {
            return canAttack;
        }
        set {
            canAttack = value;
        }
    }

    public float TimeBetweenCombos {
        get {
            return timeBetweenCombos;
        }
    }
    #endregion

    private void EnableAttack() {
        canAttack = true;
        comboCount = 0;
    }

    public void BreakAttack() {
        //evaAnimator.setBool("BreakAttack",true); //Romper ataque en el animator.
        canAttack = false;
        Invoke("EnableAttack", timeBetweenCombos);
    }

    private void Start() {
        evaAnimator = gameObject.GetComponent<Animator>();
        evaStats = gameObject.GetComponent<BaseStatsComponent>() as BaseStatsComponent;
        if (!evaStats) {
            Debug.Break();
            Debug.LogError("ERROR: GameObject has no Stats Component");
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            if (canAttack) {
                if (comboCount == 0) {
                    comboCount = 1;
                    lastAttack = Time.time;
                }
                if (comboCount < maxCombo) {
                    if (Time.time < lastAttack + maxTimeBetweenAttacks) {
                        comboCount += 1;
                    }
                }
            }
        }
        if (Time.time > lastAttack + maxTimeBetweenAttacks) {
            canAttack = false;
            Invoke("EnableAttack", timeBetweenCombos);
        }
    }
}
