using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    
    [SerializeField] private float upGravity = -10;
    [SerializeField] private float downGravity = -20;
    
    [SerializeField] private bool isGrounded;
    private bool _jumpRequested;
    
    private Vector2 _charMove;
    
    
    private Rigidbody2D _body;
    
    
    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        _body.linearVelocityX = _charMove.x * movementSpeed;
        
        if (_jumpRequested)
        {
            _body.linearVelocityY = jumpForce;
            _jumpRequested = false;
        }
    }


    public void Move(Vector2 moveInput)
    {
        _charMove = moveInput;
    }

    public void Jump()
    {
        _jumpRequested = true;
    }
}
