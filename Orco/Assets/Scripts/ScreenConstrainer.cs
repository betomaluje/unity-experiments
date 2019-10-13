using UnityEngine;

public class ScreenConstrainer : MonoBehaviour
{
    [SerializeField] private Camera mCamera;

    private void Start()
    {
        if (mCamera == null)
        {
            mCamera = Camera.main;
        }
    }

    void Update()
    {
        var pos = mCamera.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = mCamera.ViewportToWorldPoint(pos);
    }
}
