using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent
{
    public GameObject target;
    public float damage;

    public AttackEvent(GameObject t, float d)
    {
        this.target = t;
        this.damage = d;
    }
}
