using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabbing : MonoBehaviour
{
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            anim.SetBool("isAttacking", true);
        } else
        {
            anim.SetBool("isAttacking", false);
        }
    }
}
