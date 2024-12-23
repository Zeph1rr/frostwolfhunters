using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    private const string IS_RUNNING = "IsRunning";

    private void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        _animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection() {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPosition.x) {
            _spriteRenderer.flipX = true;
        } else {
            _spriteRenderer.flipX = false;
        }
    }
}
