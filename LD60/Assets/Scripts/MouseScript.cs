using UnityEngine;
using UnityEngine.InputSystem;

public class MouseScript : MonoBehaviour
{

    [SerializeField] private float followSpeed = 1f;
    
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private GameObject jumpButton;
    [SerializeField] private GameObject attackButton;
    

    // Update is called once per frame
    void Update()
    {
        var mousePos = Mouse.current.position.ReadValue();
        var targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
        
        var newPos = Vector2.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        transform.position = newPos;

    }
}
