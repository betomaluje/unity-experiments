using System.Collections;
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

    public void TornApart()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Debug.Log("TornApart");

        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("isDying", true);
        }        
        StartCoroutine(DelayDying());
    }

    private IEnumerator DelayDying()
    {
        yield return new WaitForSeconds(0.3f);
        Die();
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
