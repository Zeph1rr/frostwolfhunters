using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    private PlayerInputActions _playerInputActions;
    public void Initialize()
    {
        _playerInputActions = new PlayerInputActions();
    }

    private void HandlePlayerDeath(object sender, System.EventArgs e) {
        _playerInputActions.Disable();
    }

    private void OnEnable()
    {
        // Включаем контролы
        _playerInputActions.Enable();
    }

    private void OnDisable()
    {
        // Отключаем контролы
        _playerInputActions.Disable();
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

