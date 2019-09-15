using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float timeForToggle = 1f;
    private Transform bar;

    private bool isVisible = false;
    
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
        isVisible = true;
        gameObject.SetActive(true);
    }

    private void DoDisappear()
    {
        isVisible = false;
        gameObject.SetActive(false);
    }
}
