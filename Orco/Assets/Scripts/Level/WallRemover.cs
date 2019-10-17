using UnityEngine;

public class WallRemover : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask removalLayer;

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (CheckLayerMask(hitInfo.gameObject))
        {
            Destroy(gameObject);
        }        
    }

    private bool CheckLayerMask(GameObject target)
    {
        return (removalLayer & 1 << target.layer) == 1 << target.layer;
    }
}
