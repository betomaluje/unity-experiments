using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, MainInputActions.IPlayerActions
{
    [Header("Events")]
    public GameEvent horizontalEvent;
    public GameEvent verticalEvent;
    public GameEvent jumpEvent;
    public GameEvent absorbEvent;
    public GameEvent removeSkillEvent;

    private MainInputActions mainInputActions;
    private Vector2 movement;

    #region Lifecycle methods
    private void Awake()
    {
        mainInputActions = new MainInputActions();
        mainInputActions.Player.SetCallbacks(this);

        mainInputActions.Player.Absorb.started -= OnAbsorb;
    }

    private void OnEnable()
    {
        mainInputActions.Enable();
    }

    private void OnDisable()
    {
        mainInputActions.Disable();
    }
    #endregion

    #region Input callbacks

    public void OnMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();

        horizontalEvent.sentFloat = movement.x;
        verticalEvent.sentFloat = movement.y;

        horizontalEvent.Raise();
        verticalEvent.Raise();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        float jump = (float)context.ReadValueAsObject();

        if (jump == 1)
        {
            jumpEvent.Raise();
        }
    }

    public void OnAbsorb(InputAction.CallbackContext context)
    {
        float absorb = (float)context.ReadValueAsObject();

        if (absorb == 1)
        {
            // if we are pressing down and simultaneously pressing absorb, we want to remove the skill
            if (movement.y == -1)
            {
                removeSkillEvent.Raise();
            }
            else
            {
                absorbEvent.Raise();
            }
        }
    }

    #endregion
}
