using UnityEngine;

public class HumanDeath : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask deathLayer;

    [SerializeField] private GameEvent humanDeath;

    [SerializeField] private GameObject[] bloodSplatters;
    [SerializeField] private GameObject deathParticles;    

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (TriggerUtils.CheckLayerMask(deathLayer, hitInfo.gameObject))
        {            
            ShowParticles();
            humanDeath.Raise();
            Die();
        }
    }

    public void TornApart()
    {
        ShowParticles();

        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("isDying", true);
        }
        humanDeath.Raise();
    }

    private void ShowParticles() {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Instantiate(bloodSplatters[Random.Range(0, bloodSplatters.Length)], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
    }

    private void Die()
    {        
        Destroy(gameObject);
    }    
}
