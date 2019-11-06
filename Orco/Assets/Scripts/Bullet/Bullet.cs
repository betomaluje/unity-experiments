using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject particles;
    [SerializeField] private float speed;
    [SerializeField] private GameEvent damageEvent;    

    private Vector2 target;
    private Vector2 position;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.position;
        position = gameObject.transform.position;

        Vector2 dir = target - position;
        rb.velocity = dir * speed;        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TriggerUtils.CheckLayerMask(playerLayer, collision.gameObject))
        {
            Instantiate(particles, transform.position, Quaternion.identity);

            damageEvent.sentFloat = 1f;
            damageEvent.Raise();
        }

        if (!collision.CompareTag("Human"))
        {
            DestroyBullet();
        }        
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
