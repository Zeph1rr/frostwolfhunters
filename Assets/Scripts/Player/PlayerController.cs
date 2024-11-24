using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    [Header("Character Stats")]
    [SerializeField] private CharacterStats _characterStats;

    public void Move(Vector3 direction) {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, _characterStats.stats.Speed * Time.deltaTime);
    }

}
