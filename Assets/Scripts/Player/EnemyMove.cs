using System;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = new Vector2(_moveDirection.x, _moveDirection.y);
    }
}
