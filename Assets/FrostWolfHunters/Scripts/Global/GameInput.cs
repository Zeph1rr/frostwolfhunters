using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnAttackPressed;
    private PlayerInputActions _playerInputActions;
 
    public void Initialize()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        
        _playerInputActions.Player.Attack.performed += Attack_performed;
    }

    private void Attack_performed(InputAction.CallbackContext context) {
        OnAttackPressed?.Invoke(this, EventArgs.Empty);
    }

    private void HandlePlayerDeath(object sender, EventArgs e) {
        _playerInputActions.Disable();
    }

    private void OnDisable()
    {
        // Отключаем контролы
        _playerInputActions.Disable();
        _playerInputActions.Player.Attack.performed -= Attack_performed;
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

