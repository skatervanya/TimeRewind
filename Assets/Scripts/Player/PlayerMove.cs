using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    
    private float _horizontalMovement;
    private float _verticalMovement;

    private void Update()
    {

    }
    
    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = new Vector2(_horizontalMovement * _speed, _verticalMovement * _speed);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _horizontalMovement = context.ReadValue<Vector2>().x;
        _verticalMovement = context.ReadValue<Vector2>().y;
    }
}
