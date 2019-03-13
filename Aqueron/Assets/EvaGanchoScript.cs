using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaGanchoScript : MonoBehaviour
{
    public GameObject gancho;
    public Transform ganchoTrans;
    private bool attackDone;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !attackDone)
        {
            Instantiate(gancho, ganchoTrans.position, ganchoTrans.rotation);
            attackDone = true;
            Invoke("AttackDone", 0.5f);
        }
    }
    
    void AttackDone() {
        attackDone = false;
    }

}
