using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Disappear : MonoBehaviour
{
    [SerializeField] private float timeForDisappear = 2f;
    [SerializeField] private float timeOfEffect = 1f;

    [SerializeField] private GameObject particles;
    
    void Start()
    {
        StartCoroutine(PerformDisappear());
    }

    private IEnumerator PerformDisappear() {
        yield return new WaitForSeconds(timeForDisappear);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) {
            Instantiate(particles, transform.position, Quaternion.identity);
            sr.DOFade(0, timeOfEffect)
            .OnComplete(() =>
                {
                    Destroy(gameObject);                
                });
        }
    }
}
