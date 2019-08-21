using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    public GameObject particles;

    private PlayerStats playerStats;

    private Color playerColor;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        playerColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DeathCollider"))
        {
            changeParticlesColor(Instantiate(particles, playerStats.gameObject.transform.position, Quaternion.identity));

            playerStats.Die();
        }
    } 

    private void changeParticlesColor(GameObject deathParticles)
    {
        foreach (Transform child in deathParticles.transform)
        {
            ParticleSystem ps = child.gameObject.GetComponent<ParticleSystem>();

            ParticleSystem.MainModule ma = ps.main;

            ma.startColor = playerColor;
        }
    }
}
