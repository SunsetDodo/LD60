using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class InputHandler : MonoBehaviour
{

    
    
    private InputAction _move;
    private InputAction _jump;
    
    private PlayerMovement _player;
    
    private void Awake()
    {
        _move = InputSystem.actions.FindAction("Move");
        _jump = InputSystem.actions.FindAction("Jump");

        _jump.performed += PerformJump;
        
        _player =  GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        
    }


    private void PerformJump(InputAction.CallbackContext context)
    {
        _player.Jump();        
    }
    
    // Update is called once per frame
    private void Update()
    {
        var moveInput = _move.ReadValue<Vector2>();
        _player.Move(moveInput);
    }
}
