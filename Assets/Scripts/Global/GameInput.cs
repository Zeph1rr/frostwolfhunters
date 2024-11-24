using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

