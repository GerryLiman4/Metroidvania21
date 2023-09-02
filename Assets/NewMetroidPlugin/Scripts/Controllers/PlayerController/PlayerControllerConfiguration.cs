using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
public class PlayerControllerConfiguration : InputController
{
    private PlayerInputActions _inputActions;
    private bool _isJumping;
    private bool isDashing;
    private bool isAttacking;

    public override event Action Attack;
    private void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Gameplay.Enable();
        _inputActions.Gameplay.Jump.started += JumpStarted;
        _inputActions.Gameplay.Jump.canceled += JumpCanceled;
        _inputActions.Gameplay.Dash.started += DashStarted;
        _inputActions.Gameplay.Dash.canceled += DashCanceled;
        _inputActions.Gameplay.Attack.started += AttackStarted;
        _inputActions.Gameplay.Attack.canceled += AttackCanceled;
    }

    private void OnDisable()
    {
        _inputActions.Gameplay.Disable();
        _inputActions.Gameplay.Jump.started -= JumpStarted;
        _inputActions.Gameplay.Jump.canceled -= JumpCanceled;
        _inputActions.Gameplay.Dash.started -= DashStarted;
        _inputActions.Gameplay.Dash.canceled -= DashCanceled;
        _inputActions.Gameplay.Attack.started -= AttackStarted;
        _inputActions.Gameplay.Attack.canceled -= AttackCanceled;

        _inputActions = null;
    }

    private void JumpCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isJumping = false;
    }
    private void JumpStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isJumping = true;
    }
    private void DashCanceled(InputAction.CallbackContext callback)
    {
        isDashing = false;
    }

    private void DashStarted(InputAction.CallbackContext callback)
    {
        isDashing = true;
    }
    private void AttackCanceled(InputAction.CallbackContext callback)
    {
        isAttacking = false;
    }
    private void AttackStarted(InputAction.CallbackContext callback)
    {
        isAttacking = true;
        Attack?.Invoke();
    }
    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        return _isJumping;
    }

    public override float RetrieveMoveInput(GameObject gameObject)
    {
        return _inputActions.Gameplay.Move.ReadValue<Vector2>().x;
    }

    public override bool RetrieveDashInput(GameObject gameObject)
    {
        return isDashing;
    }

    public override bool RetrieveAttackInput(GameObject gameObject)
    {
        return isAttacking;
    }
    private void OnDestroy()
    {
        Attack = null;
    }
}

