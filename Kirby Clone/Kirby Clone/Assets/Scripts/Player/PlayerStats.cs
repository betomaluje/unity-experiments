using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerStats : MonoBehaviour
{
    public Player player;   
    public Color damageColor = Color.red;
    
    public GameEvent updateHealth;
    public GameEvent updateLives;
    public GameEvent gameOverEvent;

    private int currentHealth;
    private int currentLives;

    private Vector3 initialPosition;

    private bool isInmune = false;

    private Rigidbody2D rb;

    // changing colors
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentLives = player.lives;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        ResetHealth();

        rb = GetComponent<Rigidbody2D>();
    }

    private void ResetHealth()
    {
        currentHealth = player.health;
        updateHealth.sentInt = currentHealth;
        updateHealth.Raise();
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
        int damage = player.health;

        applyDamage(damage);

        // we make the player inmune
        isInmune = true;

        StartCoroutine(RespawnPlayer());
    }

    public void applyDamage(int damage)
    {
        if (isInmune)
        {
            return;
        }

        isInmune = true;
        SoundManager.instance.Play("Hit");

        currentHealth -= damage;

        updateHealth.sentInt = currentHealth;
        updateHealth.Raise();

        StartCoroutine(FlashSprite());
    }

    public void applyHealth(int health)
    {
        if (isInmune)
        {
            return;
        }

        int tempHealth = currentHealth + health;

        if (tempHealth > player.health)
        {
            tempHealth = player.health;
        }

        currentHealth = tempHealth;

        updateHealth.sentInt = currentHealth;
        updateHealth.Raise();
    }

    private IEnumerator FlashSprite()
    {
        spriteRenderer.material.color = Color.white;
        Color blinkColor = new Color(255, 255, 255, 0.5f); //set a translucent version of the sprite

        for (int n = 0; n < 3; n++)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = blinkColor;
        }

        spriteRenderer.color = originalColor;
        isInmune = false;
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
