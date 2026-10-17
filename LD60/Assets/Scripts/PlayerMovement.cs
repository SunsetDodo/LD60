using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    
    [SerializeField] private float upGravity = -10;
    [SerializeField] private float downGravity = -20;
    
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private bool isGrounded;
    private bool _jumpRequested;
    private bool _attackRequested;
    
    private Vector2 _charMove;
    
    
    private Rigidbody2D _body;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    
    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }
    
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(1f, 0.01f), 0, LayerMask.GetMask("Floor"));
        HandleMovement();
        HandleAnimation();
    }


    private void HandleMovement()
    {
        _body.linearVelocityX = _charMove.x * movementSpeed;
        if (_jumpRequested && isGrounded)
        {
            _body.linearVelocityY = jumpForce;
            _jumpRequested = false;
        }
    }

    private void HandleAnimation()
    {
        if (_charMove.x < 0 && !_spriteRenderer.flipX) _spriteRenderer.flipX = true;
        if (_charMove.x > 0 && _spriteRenderer.flipX) _spriteRenderer.flipX = false;
        _animator.SetFloat("Speed", Mathf.Abs(_charMove.x));
        _animator.SetBool("isGrounded", isGrounded);
        if (_attackRequested)
        {
            _animator.SetTrigger("Attack");
            _attackRequested = false;
        }

    }
    
    public void Move(Vector2 moveInput)
    {
        _charMove = moveInput;
        Debug.Log(_charMove);
    }

    public void Jump()
    {
        _jumpRequested = true;
    }


    public void Attack()
    {
        _attackRequested = true;
    }
}
