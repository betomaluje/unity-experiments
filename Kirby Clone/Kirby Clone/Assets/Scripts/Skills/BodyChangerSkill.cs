﻿using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Body Changer Skill")]
public class BodyChangerSkill : Skill
{
    public float scaleFactor;
    public float timeForPower;

    private GameObject targetObject;
    private float originalMass;

    public override void initSkill(GameObject skillObject)
    {
        targetObject = skillObject;

        Rigidbody2D rb = targetObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            originalMass = rb.mass;
        }
    }

    public override void performSkill(int direction)
    {        
    }

    public override IEnumerator performCorroutineSkill(int direction)
    {
        ChangeMass(scaleFactor);
        targetObject.transform.DOScale(scaleFactor, 0.5f);
        yield return new WaitForSeconds(timeForPower);
        ChangeMass(originalMass);
        targetObject.transform.DOScale(1, 0.5f);
    }    

    public override void setPosition(Vector3 position)
    {
        
    }

    private void ChangeMass(float mass)
    {
        Rigidbody2D rb = targetObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.mass = mass;
        }
    }
}
