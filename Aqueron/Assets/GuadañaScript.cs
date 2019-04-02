using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuadañaScript : MonoBehaviour
{
    private GameObject posicionInicial;
    private Animator playerAnim;
    private bool facingRight;
    private GameObject colgandoHand;
    [SerializeField] private float delayGuadaña = 8f;
    public static GuadañaScript Instance;
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        
        posicionInicial = GameObject.FindGameObjectWithTag("GuadañaPosicionInicial");
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        colgandoHand = GameObject.FindGameObjectWithTag("ColgandoHand");
    }

    // Update is called once per frame

    private void Update()
    {
        if (playerAnim.GetBool("Colgando") == false)
        {
            if (posicionInicial.transform.eulerAngles.y < 179 && facingRight) Flip();
            else if (posicionInicial.transform.eulerAngles.y > 179 && !facingRight) Flip();
            GetComponent<Animator>().enabled = true;
        } else if (playerAnim.GetBool("Colgando") == true)
        {
            transform.position = colgandoHand.transform.position;
            GetComponent<Animator>().enabled = false;
        }
    }
    void FixedUpdate()
    {
        if (playerAnim.GetBool("Colgando") == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionInicial.transform.position, delayGuadaña * Time.fixedDeltaTime);
        }
    }

    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }
}
