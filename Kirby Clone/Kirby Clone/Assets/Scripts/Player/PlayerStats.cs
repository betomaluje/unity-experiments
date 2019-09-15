using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Player player;   
    public Color damageColor = Color.red;
    
    public GameEvent updateLives;
    public GameEvent gameOverEvent;

    private float maxHealth;
    private float currentHealth;
    private int currentLives;

    private Vector3 initialPosition;

    private bool isInmune = false;

    private Rigidbody2D rb;
    private HealthBar healthBar;

    private void Start()
    {
        currentLives = player.lives;        
        
        healthBar = transform.Find("HealthBar").GetComponent<HealthBar>();
        rb = GetComponent<Rigidbody2D>();

        ResetHealth();
    }

    private void ResetHealth()
    {
        maxHealth = player.health;
        currentHealth = player.health;
        healthBar.setHealth(currentHealth / maxHealth);
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            // restart player position
            Die();
        }

		if (currentLives <= 0)
		{
            gameOverEvent.Raise();
		}
	}

    public void Die()
    {
        if (isInmune)
        {
            return;
        }

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //int damage = player.health;

        //applyDamage(player.health);

        // we make the player inmune
        isInmune = true;

        StartCoroutine(RespawnPlayer());
    }

    public void ApplyDamage(AttackEvent attackEvent)
    {
        if (isInmune || attackEvent.target != gameObject)
        {
            return;
        }

        isInmune = true;
        SoundManager.instance.Play("Hit");

        currentHealth -= attackEvent.damage;

        healthBar.setHealth(currentHealth / maxHealth);
        isInmune = false;
    }

    public void applyHealth(int health)
    {
        if (isInmune)
        {
            return;
        }

        float tempHealth = currentHealth + health;

        if (tempHealth > player.health)
        {
            tempHealth = player.health;
        }

        currentHealth = tempHealth;

        healthBar.setHealth(currentHealth / maxHealth);
    }

    private IEnumerator RespawnPlayer()
    {
		currentLives--;

        updateLives.sentInt = currentLives;
        updateLives.Raise();

        ResetHealth();

		yield return new WaitForSeconds(1f);

        transform.DOMove(initialPosition, 1, false).OnComplete(RestorePlayer);			
    }

    private void RestorePlayer()
    {
        // we restore the player to recieve damage
        isInmune = false;

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public int getCurrentLives()
    {
        return currentLives;
    }

    public bool isPlayerInmune()
    {
        return isInmune;
    }

    public bool isPlayerActive()
    {
        return !isInmune;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void SetPlayersPosition(Vector3 position)
    {
        initialPosition = position;
    }
}
