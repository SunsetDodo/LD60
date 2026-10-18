using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public bool isPressed = false;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    
    
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        _renderer.sprite = isPressed ? downSprite : upSprite;
        isPressed = false;
    }
}
