using UnityEngine;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    public float timeForToggle = 1f;
    private Transform bar;

    private bool isVisible = true;
    
    void Start()
    {
        bar = transform.Find("Bar");
        DoDisappear();
    }

    public void setHealth(float normalisedSize)
    {
        CancelInvoke();

        if (!isVisible)
        {
            DoAppear();
        }     
        
        if (bar != null)
        {
            bar.localScale = new Vector3(normalisedSize, 1f);
        }

        Invoke("DoDisappear", timeForToggle);
    }

    private void DoAppear()
    {
        if (isVisible)
        {
            return;
        }

        isVisible = true;
        //gameObject.SetActive(true);

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in sprites)
        {
            sr.DOFade(1, 1);
        }        
    }

    private void DoDisappear()
    {
        if (!isVisible)
        {
            return;
        }

        isVisible = false;
        //gameObject.SetActive(false);

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in sprites)
        {
            sr.DOFade(0, 1);
        }
    }
}
