using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;
    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
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

