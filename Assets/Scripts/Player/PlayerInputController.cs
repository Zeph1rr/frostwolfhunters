using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _playerInput; // Ссылка на систему ввода
    private PlayerController _playerController; // Ссылка на PlayerController

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        // Включаем контролы
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        // Отключаем контролы
        _playerInput.Disable();
    }

    private void Update()
    {
        // Получаем текущий ввод от игрока
        Vector2 moveInput = _playerInput.Player.Movement.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
            _playerController.Move(moveInput);
    }
}
