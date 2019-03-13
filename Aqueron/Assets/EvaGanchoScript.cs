using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaGanchoScript : MonoBehaviour
{
    public GameObject gancho;
    public Transform ganchoTrans;
    private bool attackDone;
    private GameObject guadaña;

    private void Start()
    {
        guadaña = GameObject.FindGameObjectWithTag("Guadaña");    
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !attackDone)
        {
            Instantiate(gancho, ganchoTrans.position, ganchoTrans.rotation);
            attackDone = true;
            Invoke("AttackDone", 0.5f);
            SpriteRenderer guadañaRenderer = guadaña.GetComponent<SpriteRenderer>();
            guadañaRenderer.enabled = false;
        }
    }
    
    void AttackDone() {
        attackDone = false;
    }

}
