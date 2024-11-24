using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
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
