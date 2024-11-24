using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player Instance {get; private set;}

    [Header("Character Stats")]
    [SerializeField] private CharacterStats _characterStats;

    private Rigidbody2D _rigidBody;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private void Awake() {
        Instance = this;
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        Vector2 direction = GameInput.Instance.GetMovementVector();
        if (Mathf.Abs(direction.x) > _minMovingSpeed || Mathf.Abs(direction.y) > _minMovingSpeed) {
            _isRunning = true;
            Move(direction);
        } else {
            _isRunning = false;
        }
        Debug.Log(_isRunning);
    }

    public void Move(Vector2 direction) {
        _rigidBody.MovePosition(_rigidBody.position + direction * (_characterStats.stats.Speed * Time.deltaTime));   
    }

    public bool IsRunning() {
        return _isRunning;
    }

}
