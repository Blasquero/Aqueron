using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaGanchoScript : MonoBehaviour
{
    public GameObject gancho;
    public Transform ganchoTrans;
    private bool attackDone;
    private GameObject guadaña;
    public PhysicsMaterial2D stickyMaterial;

    private void Start()
    {
        guadaña = GameObject.FindGameObjectWithTag("Guadaña");    
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !attackDone)
        {
            //Hace aparecer el gancho
            Instantiate(gancho, ganchoTrans.position, ganchoTrans.rotation);
            attackDone = true;
            //Cooldown gancho
            Invoke("AttackDone", 1f);

            //Guadaña detras de eva desaparece mientras se mueve en forma d egancho
            SpriteRenderer guadañaRenderer = guadaña.GetComponent<SpriteRenderer>();
            guadañaRenderer.enabled = false;

            //Cuando lanzas el gancho, eva no puede moverse
            gameObject.GetComponent<EvaMovement>().enabled = false;
            //Cuando lanza el gancho, cambia el material de eva haciendo que se adiera a las paredes, en EvaMovement al detectar suelo, devuelve el material original
            EvaMovement.Instance.circleCollider.sharedMaterial = stickyMaterial;
        }
    }
    
    //Metodo para usar en invoke para cooldown entre ganchos
    void AttackDone() {
        attackDone = false;
    }

}
