using UnityEngine;

public class WallRemover : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask removalLayer;
    [SerializeField] private bool removeThis = true;

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (TriggerUtils.CheckLayerMask(removalLayer, hitInfo.gameObject))
        {
            Debug.Log(gameObject + " touching " + hitInfo.gameObject);
            //Debug.Log("Removing wall! " + gameObject);
            if (removeThis)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(hitInfo.gameObject);

                CompositeCollider2D collider = gameObject.GetComponentInChildren<CompositeCollider2D>();
                if (collider)
                {
                    Debug.Log("Disabling collider " + gameObject);
                    collider.isTrigger = false;
                }
            }
        }        
    }    
}
