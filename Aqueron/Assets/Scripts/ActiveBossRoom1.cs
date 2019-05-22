using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBossRoom1 : MonoBehaviour
{
    [SerializeField] private GameObject door1;
    [SerializeField] private GameObject door2;
    [SerializeField] private GameObject boss;
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && activated == false) {
            door1.tag = "LockedDoor";
            door2.tag = "LockedDoor";
            boss.SetActive(true);
            activated = true;
        }
    }

    private void Update() {
        if(activated == true) {
            if(boss == null) {
                door1.tag = "UnlockedDoor";
                door2.tag = "UnlockedDoor";
                Destroy(gameObject);
            }
        }
    }

}
