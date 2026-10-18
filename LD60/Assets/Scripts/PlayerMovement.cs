using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float downPush = 0f;


    [SerializeField] private GameObject hurtBox;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float deathTimer = 1f;

    [SerializeField] private Sprite tombSprite;
    
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip[] deathSounds;

    public GameboyControl control;

    private float currentDeathTimer = -1f;
    
    
    private bool _jumpRequested;
    private bool _attackRequested;
    
    private Vector2 _charMove;
    
    
    private Rigidbody2D _body;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }
    
    private void FixedUpdate()
    {
        if (currentDeathTimer == -1f)
        {
            isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(1f, 0.01f), 0,
                LayerMask.GetMask("Floor"));
            HandleMovement();
            HandleAnimation();
        }
        else if (currentDeathTimer > 0f)
        {
            currentDeathTimer -= Time.deltaTime;
            _spriteRenderer.color = Color.Lerp(Color.red, Color.white, currentDeathTimer / deathTimer);
        }
        else
        {
            _animator.enabled = false;
            _body.mass = 100000;
            gameObject.layer = LayerMask.NameToLayer("Walls");
            _spriteRenderer.color = Color.white;
            _spriteRenderer.sprite = tombSprite;
        }
    }


    private void HandleMovement()
    {
        _body.linearVelocityX = _charMove.x * movementSpeed;
        if (_jumpRequested && isGrounded)
        {
            _audioSource.clip = jumpSounds[Random.Range(0, jumpSounds.Length)];
            _audioSource.Play();
            _body.linearVelocityY = jumpForce;
            _jumpRequested = false;
        }

        if (!isGrounded && Math.Abs(_body.linearVelocityY) < 0.1f)
        {
            _body.AddForce(new Vector2(0, -downPush));
        }
    }

    private void HandleAnimation()
    {
        if (_charMove.x < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
            hurtBox.transform.localPosition = new Vector2(-0.9f, hurtBox.transform.localPosition.y);
        }
        if (_charMove.x > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
            hurtBox.transform.localPosition = new Vector2(0, hurtBox.transform.localPosition.y);
        }
        _animator.SetFloat("Speed", Mathf.Abs(_charMove.x));
        _animator.SetBool("isGrounded", isGrounded);
        if (_attackRequested)
        {
            _animator.SetTrigger("Attack");
            _audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
            _audioSource.Play();
            _attackRequested = false;
        }

    }
    
    public void Move(Vector2 moveInput)
    {
        _charMove = moveInput;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            _jumpRequested = true;
        }
    }


    public void Attack()
    {
        _attackRequested = true;
    }



    public void Die()
    {
        _animator.SetTrigger("Die");
        _audioSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
        _audioSource.Play();
        currentDeathTimer = deathTimer;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<EnemyMovement>().isAlive() && currentDeathTimer == -1f)
        {
            Die();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            control.WinGame();
        }
    }
}
