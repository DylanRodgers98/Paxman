using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementBehaviour))]
public class MovementByPlayerInput : MonoBehaviour
{
    private MovementBehaviour _movementBehaviour;
    
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movementBehaviour.SetDirection(context.ReadValue<Vector2>());
        }
    }

    private void Start()
    {
        _movementBehaviour = GetComponent<MovementBehaviour>();
    }
}
