using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput
{
    public event EventHandler OnAttackPressed;
    public event EventHandler OnUltPressed;
    public event EventHandler OnPausePressed;
        
    private readonly PlayerInputActions _playerInputActions;
 
    public GameInput()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();

        _playerInputActions.Player.Attack.performed += Attack_performed;
        _playerInputActions.Player.Ult.performed += Ult_performed;
        _playerInputActions.Global.Escape.performed += Pause_performed;
    }

    public void Disable()
    {
        OnDisable();
    }

    private void Ult_performed(InputAction.CallbackContext context)
    {
        OnUltPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_performed(InputAction.CallbackContext context) {
        OnAttackPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(InputAction.CallbackContext context) 
    {
        OnPausePressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
        _playerInputActions.Player.Attack.performed -= Attack_performed;
        _playerInputActions.Global.Escape.performed -= Pause_performed;
    }

    public Vector2 GetMovementVector() {
        Vector2 inputVector = _playerInputActions.Player.Movement.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePosition() {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
}

