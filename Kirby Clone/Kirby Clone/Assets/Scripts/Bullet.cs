using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string targetTag;
    public GameEvent hitEvent;

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.CompareTag(targetTag))
        {
            hitEvent.sentFloat = Random.Range(1, 10);
            hitEvent.Raise();                    
        }

        // we destroy this bullet
        Destroy(gameObject);
    }
}
