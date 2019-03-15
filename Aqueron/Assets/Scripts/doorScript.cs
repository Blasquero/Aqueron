using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    public bool locked;
    public bool sensor;

    private Animator anim;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        sensor = true;
       
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        sensor = false;
        
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("locked",locked);
        anim.SetBool("sensor", sensor);
    }
}
