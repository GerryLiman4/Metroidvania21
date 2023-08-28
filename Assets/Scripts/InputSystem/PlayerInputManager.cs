using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerInputManager : MonoBehaviour
{
    [Header("In Game Controller")]
    [Header("Character Input Values")]
    public Vector2 move;
    public bool crouch;

    public event Action Jump;
    public event Action Dash;
    public event Action Shoot;
    public event Action Inventory;
    public event Action Map;
    public event Action Interact;

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
    public void OnInventory(InputValue value)
    {
        InventoryInput(value.isPressed);
    }
    public void OnMap(InputValue value)
    {
        MapInput(value.isPressed);
    }
    public void OnInteract(InputValue value)
    {
        InteractInput(value.isPressed);
    }
#endif

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        Jump?.Invoke();
    }

    public void DashInput(bool newDashState)
    {
        Dash?.Invoke();
    }
    public void CrouchInput(bool newCrouchState)
    {
        crouch = newCrouchState;
    }
    public void ShootInput(bool newShootState)
    {
        Shoot?.Invoke();
    }
    private void InventoryInput(bool isPressed)
    {
        Inventory?.Invoke();
    }
    private void MapInput(bool isPressed)
    {
        Map?.Invoke();
    }
    private void InteractInput(bool isPressed)
    {
        Interact?.Invoke();
    }
    public void PauseInput(bool isPressed)
    {
        if (isPressed){}
    }

    private void SelectInput(bool isPressed)
    {
       
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
    private void OnDestroy()
    {
        Jump = null;
        Dash = null;
        Shoot = null;
        Inventory = null;
        Map = null;
        Interact = null;
    }
}
