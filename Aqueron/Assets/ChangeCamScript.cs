using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamScript : MonoBehaviour
{
    public GameObject cam2;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == player)
        {
            cam2.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            cam2.SetActive(false);
        }
    }
}
