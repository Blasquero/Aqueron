using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    private Animator animator;
    private bool MapMenu;

    void Start() {
        MapMenu = false;
        animator = GetComponent<Animator>();
    }

    void Update() {
        if(Input.GetButtonDown("Mapa") && !MapMenu) {
            animator.enabled = true;
            animator.SetBool("MapaMenu", true);
        }
        if (Input.GetButtonDown("Mapa") && MapMenu) {
            animator.SetBool("MapaMenu", false);
        }
    }

    public void CambioAMapMenu() {
        MapMenu = !MapMenu;
    }
}
