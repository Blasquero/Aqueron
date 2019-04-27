using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LlamaSaltoScript : MonoBehaviour
{
    public static LlamaSaltoScript instance;

   public void Start()
    {
        instance = this;
    }

    public void AutoDestruccion()
    {
        gameObject.SetActive(false);
    }
}
