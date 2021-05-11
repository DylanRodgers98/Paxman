using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementBehaviour))]
public class MovementByInput : MonoBehaviour
{
    private MovementBehaviour _movementBehaviour;
    
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeScale > 0)
        {
            _movementBehaviour.SetDirection(context.ReadValue<Vector2>());
        }
    }

    private void Start()
    {
        _movementBehaviour = GetComponent<MovementBehaviour>();
    }
}
