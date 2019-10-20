using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private float distance;

    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {    
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, distance);

        lineRenderer.SetPosition(0, transform.position);

        if (hit.collider)
        {
            // if we hit something            
            lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y, transform.position.z));
        } else
        {
            lineRenderer.SetPosition(1, Vector2.up * distance);
        }        

        

        if (Input.GetKey(KeyCode.Space))
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }    
}
