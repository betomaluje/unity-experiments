using UnityEngine;
using UnityEngine.InputSystem;

public class MiniMapVisibility : MonoBehaviour
{
    private GameObject miniMap;

    private bool isMiniMapEnabled = true;

    // Start is called before the first frame update
    void Awake()
    {
        isMiniMapEnabled = true;
        
        miniMap = GameObject.FindGameObjectWithTag("MiniMap");
    }

    private void OnMapToggle()
    {
        isMiniMapEnabled = !isMiniMapEnabled;
        miniMap.SetActive(isMiniMapEnabled);
    }  
}
