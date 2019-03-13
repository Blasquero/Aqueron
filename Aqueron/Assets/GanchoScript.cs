using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GanchoScript : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private GameObject player;
    private int groundLayer;
    private bool ganchoActivo;
    private bool deberiaMoverse;
    private bool hit;
    [SerializeField] private float moveSpeed = 15f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
        Invoke("AutoDestruccion", 0.25f);
        Physics2D.IgnoreLayerCollision(12, 9);
        Physics2D.IgnoreLayerCollision(12, 10);
        deberiaMoverse = true;
        playerRb = player.GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 1 &&
            Mathf.Abs(player.transform.position.y - transform.position.y) < 1)
        {
            Destroy(gameObject);
            playerRb.gravityScale = 7;
            playerRb.velocity = Vector3.zero;
            ganchoActivo = false;
        }

        if(deberiaMoverse)
            transform.Translate(new Vector2(0f, 20f) * Time.deltaTime);

        if (ganchoActivo) {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, moveSpeed * Time.deltaTime);
            playerRb.gravityScale = 0;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == groundLayer)
        {
            ganchoActivo = true;
            transform.Translate(Vector3.zero);
            deberiaMoverse = false;
            hit = true;
        }
    }

    void AutoDestruccion()
    {
        if(!hit)
        Destroy(gameObject);

        player.GetComponent<Rigidbody2D>().gravityScale = 7;
    }
   
}
