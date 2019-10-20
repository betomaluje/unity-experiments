using UnityEngine;

public class WallRemover : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask removalLayer;

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (TriggerUtils.CheckLayerMask(removalLayer, hitInfo.gameObject))
        {
            Destroy(gameObject);
        }        
    }    
}
