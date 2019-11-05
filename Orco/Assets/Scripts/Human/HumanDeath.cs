using UnityEngine;
using System.Collections;

public class HumanDeath : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask deathLayer;
    [SerializeField] private LayerMask throwableDeathLayer;

    [SerializeField] private GameEvent humanDeath;

    [SerializeField] private GameObject[] bloodSplatters;
    [SerializeField] private GameObject deathParticles;

    [SerializeField] private float timeToResetThrow = 1f;

    [HideInInspector]
    public bool isBeingThrown = false;

    private HumanMovement humanMovement;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        humanMovement = GetComponent<HumanMovement>();
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (TriggerUtils.CheckLayerMask(deathLayer, hitInfo.gameObject))
        {
            PerformDie();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBeingThrown && TriggerUtils.CheckLayerMask(throwableDeathLayer, collision.gameObject))
        {
            PerformDie();

            HumanDeath otherHumanDeath = collision.gameObject.GetComponent<HumanDeath>();
            if (otherHumanDeath != null)
            {
                otherHumanDeath.PerformDie();
            }
        }    
    }

    public void PerformDie()
    {
        ShowParticles();
        humanDeath.Raise();
        SoundManager.instance.PlayRandom("Human Death");
        Die();
    }

    public void TornApart()
    {
        ShowParticles();
        SoundManager.instance.PlayRandom("Human Death");
        boxCollider.enabled = false;

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

    public void Throw()
    {
        StartCoroutine(ToggleThrowTimer());
    }

    private IEnumerator ToggleThrowTimer()
    {
        isBeingThrown = true;
        yield return new WaitForSeconds(timeToResetThrow);        
        isBeingThrown = false;
    }

    private void Die()
    {        
        Destroy(gameObject);
    }    
}
