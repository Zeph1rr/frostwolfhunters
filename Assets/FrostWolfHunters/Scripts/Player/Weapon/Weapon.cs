using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private string _name;
    private Player _player;
    private PolygonCollider2D _attackCollider;
    private int _damage;
    
    public string Name => _name;

    public void Initialize(Player player, int damage)
    {
        _player = player;
        _damage = damage;
        _attackCollider = GetComponent<PolygonCollider2D>();
        _player.OnPlayerAttack += HandlePlayerAttack; 
    }

    private void OnDestroy()
    {
        _player.OnPlayerAttack -= HandlePlayerAttack;        
    }

    private void HandlePlayerAttack(object sender, EventArgs damage)
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.25f);
        _attackCollider.enabled = true;
        yield return new WaitForSeconds(0.25f);
        _attackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
        }
    }
}
