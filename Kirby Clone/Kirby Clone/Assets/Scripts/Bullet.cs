using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private GameEvent hitEvent;

    [SerializeField] private float timeForDisappear = 0.5f;

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (CheckLayerMask(hitInfo.gameObject))
        {        
            float damage = Random.Range(1, 10);
            hitEvent.sentAttackEvent = new AttackEvent(hitInfo.gameObject, damage);
            hitEvent.Raise();

            // we destroy this bullet
            Destroy(gameObject);
            SoundManager.instance.Play("BulletExplosion");
        } else {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) {        
                sr.DOFade(0, timeForDisappear)
                .OnComplete(() =>
                    {
                        Destroy(gameObject);                
                    });
            }
        }
    }

    private bool CheckLayerMask(GameObject target)
    {
        return (targetLayerMask & 1 << target.layer) == 1 << target.layer;
    }
}
