using UnityEngine;

public class HumanInvinceble : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private bool playerNear = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TriggerUtils.CheckLayerMask(playerLayer, collision.gameObject))
        {
            playerNear = true;
        }
    }    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TriggerUtils.CheckLayerMask(playerLayer, collision.gameObject))
        {
            playerNear = false;
        }
    }    

    public bool IsPlayerNear()
    {
        return playerNear;
    }
}
