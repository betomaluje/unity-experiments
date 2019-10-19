﻿using System.Collections;
using UnityEngine;

public class PlayerGrabbing : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask grabbableLayer;

    [Space]
    [Header("Settings")]
    // the players item grab position
    [SerializeField] private Transform itemGameObjectPosition;
    [SerializeField] private float throwForce = 250f;

    [SerializeField] private float timeForLongPress = 0.5f;

    private Animator anim;
    private Rigidbody2D rb;

    private bool objectGrabbed = false;
    private GameObject targetObject;    
    private float timePressing = 0f;

    // animations
    private bool isTorning = false;
    private bool buttonPressed = false;
    private bool buttonUp = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        Animate();
        CheckInput();        
    }

    private void CheckInput()
    {
        buttonPressed = Input.GetButton("Fire1");
        buttonUp = Input.GetButtonUp("Fire1");

        if (buttonPressed)
        {
            timePressing += Time.deltaTime;

            if (targetObject != null)
            {
                DoGrab();
            }
        }

        if (buttonUp && targetObject != null)
        {
            if (timePressing >= timeForLongPress && !isTorning)
            {
                Debug.Log("Long press!");
                isTorning = true;
                DoTorn();
            }
            else
            {
                Debug.Log("Normal press!");
                DoThrow(itemGameObjectPosition.position - transform.position);
            }
            timePressing = 0;
        }
    }

    private void Animate()
    {
        anim.SetBool("isAttacking", buttonPressed);
        anim.SetBool("isTorning", isTorning);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (CheckLayerMask(hitInfo.gameObject) && itemGameObjectPosition.childCount == 0)
        {
            targetObject = hitInfo.gameObject;
        }
    }

    private void PutItemAsChild(GameObject item)
    {
        item.transform.parent = null;
        item.transform.parent = itemGameObjectPosition;
        item.transform.localPosition = Vector3.zero;

        Quaternion rotation = Quaternion.Euler(0, itemGameObjectPosition.rotation.y, 0);
        item.transform.rotation = rotation;
    }

    private void DoTorn()
    {
        if (targetObject == null)
        {
            Debug.Log("Fail torning");
            return;
        }                

        HumanDeath humanDeath = targetObject.GetComponent<HumanDeath>();
        if (humanDeath != null)
        {
            Debug.Log("Torning human!");
            humanDeath.TornApart();
        }

        objectGrabbed = false;

        targetObject.transform.parent = null;
        targetObject = null;
        isTorning = false;
    }

    public void DoGrab()
    {
        objectGrabbed = true;
        PutItemAsChild(targetObject);
    }

    public void DoThrow(Vector3 dir)
    {
        if (targetObject == null) return;

        objectGrabbed = false;    

        Rigidbody2D targetRb = targetObject.GetComponent<Rigidbody2D>();

        if (targetRb != null)
        {
            targetRb.AddForce(dir * throwForce, ForceMode2D.Impulse);
        }       

        targetObject.transform.parent = null;
        targetObject = null;
    }

    private bool CheckLayerMask(GameObject target)
    {
        return (grabbableLayer & 1 << target.layer) == 1 << target.layer;
    }
}
