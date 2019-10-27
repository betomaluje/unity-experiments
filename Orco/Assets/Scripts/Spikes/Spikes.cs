using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private LayerMask killableLayer;
    [SerializeField] private float targetRadius = 0.25f;
    [SerializeField] private Vector2 targetOffset;
    [SerializeField] private Color debugColor;

    private Collider2D onTargetDetected;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        onTargetDetected = Physics2D.OverlapCircle((Vector2)transform.position + targetOffset, targetRadius, killableLayer);

        anim.SetBool("isAttacking", onTargetDetected);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = debugColor;

        Gizmos.DrawWireSphere((Vector2)transform.position + targetOffset, targetRadius);
    }
}
