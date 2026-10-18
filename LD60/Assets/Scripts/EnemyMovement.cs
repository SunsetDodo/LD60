using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private GameObject wallCheck;
    [SerializeField] private Vector2 deathForce;
    [SerializeField] private float deathTime = 0.5f;
    [SerializeField] private AudioClip[] deathSounds;

    private bool facingRight = false;
    
    private Rigidbody2D _body;
    private BoxCollider2D _mainCollider;
    private CapsuleCollider2D _deathCollider;
    private SpriteRenderer _spriteRenderer;
    private float currentDeathTimer = -1f;
    private AudioSource _audioSource;
    
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _body = GetComponent<Rigidbody2D>();
        _mainCollider = GetComponent<BoxCollider2D>();
        _deathCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        _mainCollider.enabled = true;
        _deathCollider.enabled = false;
    }

    private void TurnAround()
    {
        moveSpeed *= -1;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
        
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentDeathTimer == -1f)
        {
            _body.linearVelocityX = moveSpeed;

            bool foundGround =
                Physics2D.OverlapCircle(groundCheck.transform.position, 0.2f, LayerMask.GetMask("Floor"));
            bool foundWall = Physics2D.OverlapCircle(wallCheck.transform.position, 0.2f, LayerMask.GetMask("Walls")) ||
                             Physics2D.OverlapCircle(wallCheck.transform.position, 0.2f, LayerMask.GetMask("Floor"))    ||
                             Physics2D.OverlapCircle(wallCheck.transform.position, 0.2f, LayerMask.GetMask("Enemy"));

            if (!foundGround || foundWall)
            {
                TurnAround();
            }

        }
        else
        {
            currentDeathTimer -= Time.deltaTime;
            _spriteRenderer.color = Color.Lerp(Color.red, Color.white, currentDeathTimer / deathTime);
            if (currentDeathTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Hit(true);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Hit(false);
        }
    }

    public void Hit(bool fromLeft)
    {
        _audioSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
        _audioSource.Play();
        currentDeathTimer = deathTime;
        _body.mass = 0.1f;
        _mainCollider.enabled = false;
        _deathCollider.enabled = true;
        _body.AddForce(new Vector2(deathForce.x  * (fromLeft ? 1 : -1), deathForce.y), ForceMode2D.Impulse);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("EnemyHurt"))
        {
            if (collision.transform.parent.position.x < transform.position.x)
            {
                Hit(true);
            }
            else
            {
                Hit(false);
            }
        }
    }


    public bool isAlive()
    {
        return _mainCollider.enabled;
    }
}
