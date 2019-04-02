using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    private Animator changeLevelFadeAnim;
    private Vector3 playerPos;
    public GameObject posicionInicial;

    private void Start()
    {
        changeLevelFadeAnim = GameObject.Find("ChangeLevelFade").GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            changeLevelFadeAnim.SetTrigger("FadeOut");
            Invoke("GuadañaTeleport", 1f);
            if (this.gameObject.name == "ChangeLevelTo1")
            {
                Invoke("EvaTeleportTo1", 1f);
            }
            if (this.gameObject.name == "ChangeLevelTo0")
            {
                Invoke("EvaTeleportTo0", 1f);
            }
        }
    }

    void ChangeLevelDelay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    void EvaTeleportTo1()
    {
        SceneManager.LoadScene(1);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(45f, 1f, 0f);
    }
    void EvaTeleportTo0()
    {
        SceneManager.LoadScene(0);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(33.65f, 1f, 0f);
    }
    
    void GuadañaTeleport()
    {
        GameObject.FindGameObjectWithTag("Guadaña").transform.position = GameObject.FindGameObjectWithTag("GuadañaPosicionInicial").transform.position;
    }
}
