using UnityEngine;

public class Trophy : MonoBehaviour
{
    [SerializeField] private LayerMask grabbableLayer;
    [SerializeField] private GameEvent youWinEvent;
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (TriggerUtils.CheckLayerMask(grabbableLayer, other.gameObject))
        {            
            youWinEvent.Raise();
        }
    }
}
