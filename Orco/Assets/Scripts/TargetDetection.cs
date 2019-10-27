using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask targetLayer;

    [Space]
    [Header("Grab")]
    [SerializeField] private float targetRadius = 0.25f;
    [SerializeField] private Vector2 targetOffset;
    [SerializeField] private Color debugColor;

    protected Collider2D onTargetDetected;

    public virtual void Update()
    {
        onTargetDetected = Physics2D.OverlapCircle((Vector2)transform.position + targetOffset, targetRadius, targetLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = debugColor;
        Gizmos.DrawWireSphere((Vector2)transform.position + targetOffset, targetRadius);
    }
}
