using UnityEngine;

public class Trophy : MonoBehaviour
{
    [SerializeField] private LayerMask grabbableLayer;
    [SerializeField] private GameEvent youWinEvent;
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (TriggerUtils.CheckLayerMask(grabbableLayer, other.gameObject))
        {        
            int score = GameObject.FindGameObjectWithTag("Player Score").GetComponent<PlayerScore>().GetPlayerScore();
            youWinEvent.sentInt = score;
            youWinEvent.Raise();
        }
    }
}
