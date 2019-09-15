using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunController : MonoBehaviour
{
    private GameObject objectToLookup;

    private void Start()
    {
        objectToLookup = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector2 direction = new Vector2(
            objectToLookup.transform.position.x - transform.position.x,
            objectToLookup.transform.position.y - transform.position.y);

        transform.right = direction;
    }
}
