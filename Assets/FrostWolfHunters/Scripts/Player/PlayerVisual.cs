using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Player _player;

    private string _weaponName;
    
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";

    public void Initialize(Player player) {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = player;
        _weaponName = _player.WeaponName;
        _player.OnPlayerDied += HandleDie;
        _player.OnPlayerAttack += HandleAttack;
    }

    private void OnDestroy() {
       _player.OnPlayerDied -= HandleDie;
       _player.OnPlayerAttack -= HandleAttack;
    }

    private void Update() {
        _animator.SetBool(IS_RUNNING, _player.IsRunning());
    }

    private void HandleDie(object sender, System.EventArgs e)
     {
        Debug.Log("Player is dead!");
        _animator.SetBool(IS_DEAD, true);
    }

    private void HandleAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger($"{_weaponName.ToUpper()}_ATTACK");
    }
}
