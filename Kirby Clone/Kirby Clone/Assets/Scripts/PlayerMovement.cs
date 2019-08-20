using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, MainInputActions.IPlayerActions
{
    public float speed = 5f;
    public MainInputActions controls;

    private void Awake()
    {
        controls = new MainInputActions();
        controls.Player.SetCallbacks(this);
    }    

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        float movement = (float) context.ReadValueAsObject();
        Vector3 direction = new Vector3(movement * speed, 0);
        Debug.Log("Player wants to move: " + direction);
        transform.position += direction;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        float movement = (float)context.ReadValueAsObject();
        Debug.Log("jump: " + movement);
    }
}
