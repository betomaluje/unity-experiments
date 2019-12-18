using UnityEngine;

public class EnemyAppear : MonoBehaviour
{
    [SerializeField] private GameEvent enemyAppearEvent;
    
    void Start()
    {
        enemyAppearEvent.Raise();
    }
    
}
