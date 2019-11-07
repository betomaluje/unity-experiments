using UnityEngine;

public class HumanDeath : IThrowableAction, IThrowableAction.IThrownCollision
{
    [Header("Layers")]
    [SerializeField] private LayerMask deathLayer;

    [SerializeField] private GameEvent humanDeath;

    [SerializeField] private GameObject[] bloodSplatters;
    [SerializeField] private GameObject deathParticles;

    private HumanMovement humanMovement;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        humanMovement = GetComponent<HumanMovement>();
        onCollisionListener = this;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (TriggerUtils.CheckLayerMask(deathLayer, hitInfo.gameObject))
        {
            PerformDie();
        }
    }

    public void OnThrownCollision(GameObject collisionObject)
    {
        Debug.Log("human collision with " + collisionObject.gameObject);

        PerformDie();

        HumanDeath otherHumanDeath = collisionObject.GetComponent<HumanDeath>();
        if (otherHumanDeath != null)
        {
            Debug.Log("other human Die");
            otherHumanDeath.PerformDie();
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

    private void Die()
    {        
        Destroy(gameObject);
    }
}
