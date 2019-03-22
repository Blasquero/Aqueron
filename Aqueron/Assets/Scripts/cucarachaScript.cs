using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cucarachaScript : MonoBehaviour
{

    private float width;
    LayerMask ground;
    private Rigidbody2D rb;
    //velocidad negativa porque el sprite está mirando hacia la izquierda
    private float velocity = -1f;
    private bool facingRight;
    void Start()
    {
        //Get Componentes
        width = GetComponent<SpriteRenderer>().bounds.extents.x;
        rb = GetComponent<Rigidbody2D>();
        //Get Layer ground
        ground = 1 << LayerMask.NameToLayer("Ground");


        // hilo de ataque
        StartCoroutine(attack());
    }

    void FixedUpdate()
    {
        //Linea de longitud .02f vertical
        Vector2 vec2 = Tovector2(transform.right) * -.02f;
        //Posicion al extremo horizontal del sprite
        Vector2 groundPos = transform.position + transform.right * -width;
        //Visualizacion de la linea vertical
        Debug.DrawLine(groundPos, groundPos + Vector2.down * 0.1f);
        //Visualizacion de la linea horizontal
        Debug.DrawLine(groundPos, groundPos + vec2);
        //LineCast vertical detecta si el enemigo esta tocando el suelo
        bool isGrounded = Physics2D.Linecast(groundPos, groundPos + Vector2.down * 3, ground);
        //Linecast horizontal detecta si el enemigo tiene un obstaculo delante
        bool isBlocked = Physics2D.Linecast(groundPos, groundPos + vec2, ground);

        //Movimiento constanate del enemigo sin aceleración
        Vector2 myVel = rb.velocity;
        myVel.x = transform.right.x * velocity;
        rb.velocity = myVel;

        //Flipeo del enemigo en funcion de la deteccion de los linecasts
        if (!isGrounded) Flip();
        if (isBlocked) Flip();
    }

    //Metodo flipeo de sprite
    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }

    //Metodo para transformar vector3 a vector2, util para el linecast horizontal del isBlocked
    private Vector3 Tovector2(Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }

    //hilo de ataque bucleado infinitamente
    IEnumerator attack()
    {
        while (true)
        {
            yield return 50000f;
            velocity = 0f;
            yield return 50000f;
            velocity = -5f;
        }
    }

}
