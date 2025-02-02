using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Player _player;

    private bool _isPlayerDead = false;
    
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";

    public void Initialize(Player player) {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = player;
        _player.OnPlayerDied += HandleDie;
    }

    private void OnDestroy() {
        Player.Instance.OnPlayerDied -= HandleDie;
    }

    private void Update() {
        _animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        if (!_isPlayerDead)
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

    private void HandleDie(object sender, System.EventArgs e) {
        Debug.Log("Player is dead!");
        _animator.SetBool(IS_DEAD, true);
        _isPlayerDead = true;
    }
}
