using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDeath : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask deathLayer;

    [SerializeField] private GameEvent humanDeath;

    [SerializeField] private GameObject deathParticles;

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (CheckLayerMask(hitInfo.gameObject))
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Die();
        }
    }

    private void Die()
    {
        humanDeath.Raise();
        Destroy(gameObject);
    }

    private bool CheckLayerMask(GameObject target)
    {
        return (deathLayer & 1 << target.layer) == 1 << target.layer;
    }
}
