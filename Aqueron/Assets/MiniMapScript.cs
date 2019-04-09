using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    private Animator animator;
    private bool MapMenu;
    // Start is called before the first frame update
    void Start()
    {
        MapMenu = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Mapa") && !MapMenu)
        {
            animator.enabled = true;
            animator.SetBool("MapaMenu", true);
        }
        if (Input.GetButtonDown("Mapa") && MapMenu)  
        {
            animator.SetBool("MapaMenu", false);
        }
    }

    public void CambioAMapMenu()
    {
        MapMenu = !MapMenu;
    }
}
