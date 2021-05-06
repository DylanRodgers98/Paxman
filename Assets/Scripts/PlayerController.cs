using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Vector2 _movementDirection = Vector2.left;
    private Vector3 _lastKnownPosition;

    private void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movementDirection = context.ReadValue<Vector2>();
            SetScale();
            SetRotation();
        }
    }

    private void SetScale()
    {
        transform.localScale = _movementDirection == Vector2.right
            ? new Vector3(-1, 1, 1) // flip sprite across x axis if facing right direction
            : Vector3.one; // keep sprite scale normal for all other directions
    }

    private void SetRotation()
    {
        if (_movementDirection == Vector2.left || _movementDirection == Vector2.right)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (_movementDirection == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (_movementDirection == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    private void Update()
    {
        if (!IsStationary()) UpdatePosition();
    }

    private bool IsStationary()
    {
        if (_movementDirection == Vector2.zero) return true;
        if (transform.position == _lastKnownPosition)
        {
            _movementDirection = Vector2.zero;
            return true;
        }

        return false;
    }

    private void UpdatePosition()
    {
        _lastKnownPosition = transform.position;
        float distanceToMove = movementSpeed * Time.deltaTime;
        float x = _lastKnownPosition.x + _movementDirection.x * distanceToMove;
        float y = _lastKnownPosition.y + _movementDirection.y * distanceToMove;
        transform.position = new Vector2(x, y);
    }
}