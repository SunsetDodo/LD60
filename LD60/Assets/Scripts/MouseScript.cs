using UnityEngine;
using UnityEngine.InputSystem;

public class MouseScript : MonoBehaviour
{

    [SerializeField] private float followSpeed = 3f;

    [SerializeField] private Vector2 horizontalBounds;
    [SerializeField] private Vector2 verticalBounds;

    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private GameObject hand;
    [SerializeField] private Vector3 pressOffset;

    private SpriteRenderer _renderer;
    private bool isPressed = false;
    
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private GameObject jumpButton;
    [SerializeField] private GameObject attackButton;
    
    [SerializeField] private PlayerMovement _player;

    private void Start()
    {
        _renderer = hand.GetComponent<SpriteRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
        var mousePos = Mouse.current.position.ReadValue();
        var targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
        
        var newPos = Vector2.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        newPos.x = Mathf.Clamp(newPos.x, horizontalBounds.x, horizontalBounds.y);
        newPos.y = Mathf.Clamp(newPos.y, verticalBounds.x, verticalBounds.y);
        transform.position = newPos;


        if (!Input.GetMouseButton(0))
        {
            _player.Move(Vector2.zero);
            if (isPressed)
            {
                isPressed = false;
                _renderer.sprite = upSprite;
                hand.transform.position += pressOffset;
            }

            return;
        }
        
        if (!isPressed)
        {
            isPressed = true;
            _renderer.sprite = downSprite;
            hand.transform.position -= pressOffset;
        }

        var buttonCollider = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Buttons"));
        if (!buttonCollider) return;
        buttonCollider.gameObject.GetComponent<ButtonPress>().isPressed = true;
        if (buttonCollider.gameObject == leftButton)  _player.Move(Vector2.left);
        if (buttonCollider.gameObject == rightButton)  _player.Move(Vector2.right);
            
        if (buttonCollider.gameObject == jumpButton && Input.GetMouseButtonDown(0))
        {
            _player.Jump();                
        }
        
        if (buttonCollider.gameObject == attackButton && Input.GetMouseButtonDown(0))
        {
            _player.Attack();                
        }
    }
}
