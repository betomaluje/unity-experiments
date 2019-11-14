using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniMapVisibility : MonoBehaviour
{
    [SerializeField] private GameObject miniMap;

    private PlayerInputActions inputActions;
    private bool isMiniMapEnabled = true;

    // Start is called before the first frame update
    void Awake()
    {
        isMiniMapEnabled = true;
        inputActions = new PlayerInputActions();

        inputActions.Player.MapToggle.performed += context => ToggleMiniMap(context);
    }

    private void ToggleMiniMap(InputAction.CallbackContext context)
    {
        isMiniMapEnabled = !isMiniMapEnabled;
        miniMap.SetActive(isMiniMapEnabled);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }    
}
