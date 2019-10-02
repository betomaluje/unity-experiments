using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Grabbable : MonoBehaviour
{
    private Rigidbody2D rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    public void DoEnable()
    {
        if (rb != null)
        {
            rb.gravityScale = 1;
        }
    }

    public void DoDisable()
    {
        if (rb != null)
        {
            rb.gravityScale = 0;
        }        
    }
}
