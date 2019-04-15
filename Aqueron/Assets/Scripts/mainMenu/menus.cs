using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menus : MonoBehaviour
{
    public GameObject panelMain;
    public GameObject panelPlay;
    public GameObject panelSettings;
    public GameObject panelCredits;


    public GameObject Cursor;
    public Image CursorReference;
    //The Color to be assigned to the Renderer’s Material
    Color m_NewColor;
    //These are the values that the Color Sliders return
     public Slider m_Red, m_Blue, m_Green;

    public void Start()
    {
        Cursor = GameObject.Find("cursor");
    }


    public void Update()
    {
        if (Cursor && CursorReference) 
        {
            CursorReference.sprite = Cursor.GetComponent<SpriteRenderer>().sprite;
            CursorReference.color = Cursor.GetComponent<SpriteRenderer>().color;
        }
    }

    public void changeColor()
    {
        m_NewColor = new Color(m_Red.value, m_Blue.value, m_Green.value);
        Cursor.GetComponent<SpriteRenderer>().color = m_NewColor;
    }

    public void playMenu()
    {
        gameObject.GetComponent<Animator>().SetInteger("menuPosition", 1);
    }
    public void Continue()
    {

    }
    public void NewGame()
    {

    }
    
    public void backMainPlay()
    {
        gameObject.GetComponent<Animator>().SetInteger("menuPosition", 0);
    }
    public void backMainSettings()
    {
        gameObject.GetComponent<Animator>().SetInteger("menuPosition", 0);
    }
    public void backMainCredits()
    {
        gameObject.GetComponent<Animator>().SetInteger("menuPosition", 0);
    }
    public void Settings()
    {
        gameObject.GetComponent<Animator>().SetInteger("menuPosition", 2);
    }
    public void Cedits()
    {
        gameObject.GetComponent<Animator>().SetInteger("menuPosition", 3);
    }
    public void Quit()
    {
        Application.Quit();
    }
}