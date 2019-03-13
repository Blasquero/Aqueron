using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GanchoScript : MonoBehaviour
{

    private GameObject player;
    private Rigidbody2D rb;
    private int groundLayer;
    private bool ganchoActivo;
    private bool deberiaMoverse;
    private bool hit;
    [SerializeField]
    private float moveSpeed = 15f;
    public PhysicsMaterial2D stickyMaterial;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
        Invoke("AutoDestruccion", 0.25f);
        Physics2D.IgnoreLayerCollision(12, 9);
        Physics2D.IgnoreLayerCollision(12, 10);
        rb = GetComponent<Rigidbody2D>();
        deberiaMoverse = true;

    }

    private void Update()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 1 &&
            Mathf.Abs(player.transform.position.y - transform.position.y) < 1)
        {
            Destroy(gameObject);
            player.GetComponent<Rigidbody2D>().gravityScale = 7;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        if(deberiaMoverse)
            transform.Translate(new Vector2(0f, 20f) * Time.deltaTime);

        if (ganchoActivo) {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, moveSpeed * Time.deltaTime);
            player.GetComponent<Rigidbody2D>().gravityScale = 0;

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
