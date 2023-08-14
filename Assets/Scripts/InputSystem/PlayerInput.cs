using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerInput : MonoBehaviour
{
    [Header("In Game Controller")]
    [Header("Character Input Values")]
    public Vector2 move;
    public bool crouch;

    public event Action Jump;
    public event Action Dash;
    public event Action Shoot;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = false;
    public bool lockInput = false;

    [Header("Menu Controller")]
    public bool select;


#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnDash(InputValue value)
    {
        DashInput(value.isPressed);
    }
    public void OnCrouch(InputValue value)
    {
        CrouchInput(value.isPressed);
    }
    public void OnShoot(InputValue value)
    {
        ShootInput(value.isPressed);
    }
    public void OnPause(InputValue value)
    {
        PauseInput(value.isPressed);
    }
    public void OnSelect(InputValue value)
    {
        SelectInput(value.isPressed);
    }

#endif

    public void MoveInput(Vector2 newMoveDirection)
    {
        Debug.Log(newMoveDirection);
        move = newMoveDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        Debug.Log(newJumpState);
        Jump?.Invoke();
    }

    public void DashInput(bool newDashState)
    {
        Debug.Log(newDashState);
        Dash?.Invoke();
    }
    public void CrouchInput(bool newCrouchState)
    {
        crouch = newCrouchState;
    }
    public void ShootInput(bool newShootState)
    {
        Debug.Log(newShootState);
        Shoot?.Invoke();
    }
    public void PauseInput(bool newPauseState)
    {
        if (newPauseState)
        {

        }
    }
    private void SelectInput(bool isPressed)
    {
        Debug.Log("Select");
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
