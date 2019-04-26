using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuadañaScript : MonoBehaviour
{
    private GameObject scytheInitialPosition;
    private Animator playerAnim;
    private bool facingRight;
    private GameObject hangingHand;
    [SerializeField] private float scytheDelay;
    public static GuadañaScript Instance;
    private float distance;
    private GameObject player;
    public ParticleSystem hangingParticles;
    private bool activatedHangingParticles;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        
        scytheInitialPosition = GameObject.FindGameObjectWithTag("GuadañaPosicionInicial");
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        hangingHand = GameObject.FindGameObjectWithTag("ColgandoHand");
        player = GameObject.FindGameObjectWithTag("Player");
        activatedHangingParticles = true;
    }

    private void Update() {
        //Si la guadaña se aleja demasiado de eva, esta aumenta su velocidad hasta que este cerca
        distance = Vector2.Distance(gameObject.transform.position, player.transform.position);

        if(distance > 3) {
            scytheDelay = 30f;
        } else {
            scytheDelay = 8f;
        }

        if (playerAnim.GetBool("Colgando") == false) {
            if (scytheInitialPosition.transform.eulerAngles.y < 179 && facingRight)
            {
                Flip();
            }
            else if (scytheInitialPosition.transform.eulerAngles.y > 179 && !facingRight)
            {
                Flip();
            }
            GetComponent<Animator>().enabled = true;
            if(activatedHangingParticles) {
                hangingParticles.Stop();
                activatedHangingParticles = false;
            }
        } else if (playerAnim.GetBool("Colgando") == true) {
            if (!activatedHangingParticles) {
                hangingParticles.Play();
                activatedHangingParticles = true;
            }
            transform.position = hangingHand.transform.position;
            GetComponent<Animator>().enabled = false;
        }
    }

    void FixedUpdate() {
        if (playerAnim.GetBool("Colgando") == false) {
            transform.position = Vector2.MoveTowards(transform.position, scytheInitialPosition.transform.position, scytheDelay * Time.fixedDeltaTime);
        }
    }

    void Flip() {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }
}
