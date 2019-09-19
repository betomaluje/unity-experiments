using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRepeatBullet : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject shootingObject;
    public float timeBetweenBullets = 1f;    
    public float detectRadius = 0.25f;
    public Vector2 radius;

    private float shootForce;
    private bool onTarget;
    private LayerMask targetLayerMask;
    private bool isOnTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        Enemy enemy = GetComponentInParent<EnemyController>().enemy;

        shootForce = enemy.shootForce;
        targetLayerMask = enemy.attackLayerMask;
    }

    private void Update()
    {
        onTarget = Physics2D.OverlapCircle((Vector2)transform.position + radius, detectRadius, targetLayerMask);

        if (onTarget && !isOnTarget)
        {
            isOnTarget = true;
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        Rigidbody2D rb = Instantiate(shootingObject, shootingPoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce(transform.right * shootForce, ForceMode2D.Impulse);        
        }        

        yield return new WaitForSeconds(timeBetweenBullets);
        isOnTarget = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + radius, detectRadius);
    }
}
