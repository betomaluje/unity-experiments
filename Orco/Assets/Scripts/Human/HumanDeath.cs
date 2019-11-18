using UnityEngine;

public class HumanDeath : ThrowableAction, ThrowableAction.IThrownCollision
{
    [Header("Layers")]
    [SerializeField] private LayerMask deathLayer;

    [Space]
    [Header("Settings")]    
    [SerializeField] private GameEvent humanDeath;    

    [Space]
    [Header("FX")]
    [SerializeField] private GameObject[] bloodSplatters;
    [SerializeField] private GameObject deathParticles;

    public GameObject attacker;

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
            RaiseDeathEvent();
            PerformDie();
        }
    }

    private void RaiseDeathEvent()
    {
        if (attacker != null)
        {
            humanDeath.sentAttackEvent = new AttackEvent(attacker, 1f);
            humanDeath.Raise();
        }
    }

    public void OnThrownCollision(GameObject collisionObject)
    {
        Debug.Log("human collision with " + collisionObject.gameObject);

        RaiseDeathEvent();
        PerformDie();

        HumanDeath otherHumanDeath = collisionObject.GetComponent<HumanDeath>();
        if (otherHumanDeath != null)
        {
            Debug.Log("other human Die");
            RaiseDeathEvent();
            otherHumanDeath.PerformDie();
        }
    }

    public void PerformDie()
    {
        ShowParticles();        
        SoundManager.instance.PlayRandom("Human Death");
        Die();
    }

    public void TornApart(GameObject attacker)
    {
        this.attacker = attacker;

        ShowParticles();
        SoundManager.instance.PlayRandom("Human Death");
        boxCollider.enabled = false;

        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("isDying", true);
        }
        RaiseDeathEvent();
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
