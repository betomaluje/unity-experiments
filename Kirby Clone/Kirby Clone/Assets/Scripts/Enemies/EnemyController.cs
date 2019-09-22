using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public Transform groundDetection;

	public Enemy enemy;
	public ParticleSystem damageParticles;
	public GameEvent hitPlayerEvent;

	private bool movingRight = true;
	private float currentHealth;
	private float maxHealth;

	private Rigidbody2D rb;
    private HealthBar healthBar;

    private void Start()
	{
		maxHealth = enemy.health;
		currentHealth = maxHealth;

		rb = GetComponent<Rigidbody2D>();
        healthBar = transform.Find("HealthBar").GetComponent<HealthBar>();        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(groundDetection.position, groundDetection.position + groundDetection.transform.right * 0.8f);
        Gizmos.DrawLine(groundDetection.position, groundDetection.position +  groundDetection.transform.up * -1 * 0.8f);
    }

    private void Update()
	{
        if (enemy.speed <= 0)
        {
            return;
        }


        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.8f, enemy.groundLayerMask);
        RaycastHit2D wallInfo = Physics2D.Raycast(groundDetection.position, Vector2.right, 0.8f, enemy.groundLayerMask);

        if (groundInfo.collider == false)
		{
			if (movingRight)
			{
				transform.Rotate(0f, 180f, 0f);
				movingRight = false;
			}
			else
			{
				transform.Rotate(0f, 0f, 0f);
				movingRight = true;
			}
		}

        int d = 1;

        if (wallInfo)
        {           
            if (movingRight)
            {
                transform.Rotate(0f, 180f, 0f);
                movingRight = false;
                d = 1;
            }
            else
            {
                transform.Rotate(0f, 0f, 0f);
                movingRight = true;
                d = -1;
            }
        }
        
        transform.Translate(Vector2.right * d * enemy.speed * Time.deltaTime);
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			StartCoroutine(PushPlayer(other.gameObject));

            float damage = Random.Range(1, 10);
            hitPlayerEvent.sentAttackEvent = new AttackEvent(other.gameObject, enemy.attack);
            hitPlayerEvent.Raise();
		}        
		else if (other.gameObject.CompareTag("Bullet"))
		{
			StartCoroutine(RecieveImpact((other.gameObject)));
		}
	}

	private IEnumerator PushPlayer(GameObject player)
	{
		var dir = player.transform.position - transform.position;
		// normalize force vector to get direction only and trim magnitude
		dir.Normalize();
		player.GetComponent<Rigidbody2D>().AddRelativeForce(dir * enemy.forceImpulse, ForceMode2D.Impulse);

		yield return new WaitForSeconds(0.2f);
	}

	private IEnumerator RecieveImpact(GameObject otherObject)
	{
		var dir = transform.position - otherObject.transform.position;
		// normalize force vector to get direction only and trim magnitude
		dir.Normalize();
		rb.AddForce(dir * 3, ForceMode2D.Impulse);

		yield return new WaitForSeconds(0.2f);
	}

	public void ApplyDamage(AttackEvent attackEvent)
	{
        if (attackEvent.target != gameObject)
        {
            return;
        }        

		//healthBar.gameObject.SetActive(true);
		currentHealth -= attackEvent.damage;

		if (currentHealth <= 0)
		{
			Instantiate(damageParticles, transform.position, Quaternion.identity);
			// die
			Destroy(gameObject);
		}

		healthBar.setHealth(currentHealth / maxHealth);
		//StartCoroutine(HideHealthBar());
	}

	private IEnumerator HideHealthBar()
	{
		yield return new WaitForSeconds(1f);
		//healthBar.gameObject.SetActive(false);
	}
}
