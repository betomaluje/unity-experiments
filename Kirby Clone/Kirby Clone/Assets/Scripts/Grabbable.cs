using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    public void DoEnable()
    {
        if (rb != null)
        {
            rb.simulated = true;
        }
    }

    public void DoDisable()
    {
        if (rb != null)
        {
            rb.simulated = false;
        }
    }
}
