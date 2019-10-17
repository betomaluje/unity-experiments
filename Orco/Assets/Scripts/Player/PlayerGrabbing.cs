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
    private bool isTorning = false;
    private float timePressing = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            anim.SetBool("isAttacking", true);

            if (targetObject != null)
            {                        
                DoGrab();                             
            }

            timePressing += Time.deltaTime;            

        } else if (Input.GetButtonUp("Fire1") && targetObject != null)
        {
            if (timePressing >= timeForLongPress && !isTorning)
            {
                Debug.Log("Long press!");
                isTorning = true;
                StartCoroutine(DoTorn());
            }
            else
            {
                Debug.Log("Normal press!");
                DoThrow(itemGameObjectPosition.position - gameObject.transform.position);
            }

            timePressing = 0;
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (CheckLayerMask(hitInfo.gameObject) && itemGameObjectPosition.childCount == 0)
        {
            targetObject = hitInfo.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D hitInfo)
    {
        if (CheckLayerMask(hitInfo.gameObject) && hitInfo.gameObject.Equals(targetObject))
        {
            targetObject = null;
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

    private IEnumerator DoTorn()
    {
        if (targetObject == null) yield return null;

        anim.SetBool("isTorning", true);

        HumanDeath humanDeath = targetObject.GetComponent<HumanDeath>();
        if (humanDeath != null)
        {
            humanDeath.TornApart();
        }

        objectGrabbed = false;

        targetObject.transform.parent = null;
        targetObject = null;

        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isTorning", false);
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
