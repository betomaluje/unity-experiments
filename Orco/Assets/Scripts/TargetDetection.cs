﻿using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask targetLayer;

    [Space]
    [Header("Grab")]
    [SerializeField] private float targetRadius = 0.25f;
    [SerializeField] private Color debugColor;

    public Collider2D onTargetDetected;

    private Vector2 dir;

    public virtual void Update()
    {
        onTargetDetected = Physics2D.OverlapCircle(GetTargetPosition(), targetRadius, targetLayer);
    }

    private Vector2 GetTargetPosition()
    {
        return (Vector2) transform.position + dir;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = debugColor;
        Gizmos.DrawWireSphere(GetTargetPosition(), targetRadius);
    }

    protected void SetDirection(Vector2 vector)
    {
        dir = vector;
    }
}
