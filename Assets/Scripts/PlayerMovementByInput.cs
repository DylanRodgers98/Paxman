using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementByInput : PlayerMovementBehaviour
{
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeScale > 0)
        {
            SetMovementDirection(context.ReadValue<Vector2>());
        }
    }
}
