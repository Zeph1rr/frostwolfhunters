<<<<<<< HEAD
using UnityEngine;

=======
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[SelectionBase]
>>>>>>> develop
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player Instance {get; private set;}

    [Header("Character Stats")]
<<<<<<< HEAD
    [SerializeField] private CharacterStats _characterStats;
=======
    [SerializeField] private PlayerStatsSO _characterStats;
>>>>>>> develop

    private Rigidbody2D _rigidBody;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;
    private Vector2 _movementVector;

<<<<<<< HEAD
    private void Awake() {
        Instance = this;
        _rigidBody = GetComponent<Rigidbody2D>();
=======
    public bool IsRunning() {
        return _isRunning;
    }

    private void Awake() {
        Instance = this;
        _rigidBody = GetComponent<Rigidbody2D>();
        _characterStats.Stats.CurrentHealth = _characterStats.Stats.MaxHealth;
>>>>>>> develop
    }

    private void Update() {
        _movementVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate() {
        if (Mathf.Abs(_movementVector.x) > _minMovingSpeed || Mathf.Abs(_movementVector.y) > _minMovingSpeed) {
            _isRunning = true;
            Move(_movementVector);
        } else {
            _isRunning = false;
        }
<<<<<<< HEAD
        Debug.Log(_isRunning);
    }

    public void Move(Vector2 direction) {
        _rigidBody.MovePosition(_rigidBody.position + direction * (_characterStats.stats.Speed * Time.deltaTime));   
    }

    public bool IsRunning() {
        return _isRunning;
=======
    }

    public void Move(Vector2 direction) {
        _rigidBody.MovePosition(_rigidBody.position + direction * (_characterStats.Stats.Speed * Time.deltaTime));
>>>>>>> develop
    }

    public Vector3 GetPlayerScreenPosition() {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

<<<<<<< HEAD
=======
    public void TakeDamage(int damage) {
        _characterStats.Stats.CurrentHealth -= damage;
        Debug.Log("Current health: " + _characterStats.Stats.CurrentHealth);
    }
>>>>>>> develop
}
