using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Vector2 _moveDirection = Vector2.left;
    
    private void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _moveDirection = context.ReadValue<Vector2>();
            SetScale();
            SetRotation();
        }
    }

    private void SetScale()
    {
        transform.localScale = _moveDirection == Vector2.right 
            ? new Vector3(-1, 1, 1) 
            : Vector3.one;
    }

    private void SetRotation()
    {
        if (_moveDirection == Vector2.left || _moveDirection == Vector2.right)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (_moveDirection == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (_moveDirection == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    private void Update()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        if (_moveDirection == Vector2.zero) return;
        Vector3 currentPosition = transform.position;
        float distanceToMove = movementSpeed * Time.deltaTime;
        float x = currentPosition.x + _moveDirection.x * distanceToMove;
        float y = currentPosition.y + _moveDirection.y * distanceToMove;
        transform.position = new Vector2(x, y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _moveDirection = Vector2.zero;
    }
}
