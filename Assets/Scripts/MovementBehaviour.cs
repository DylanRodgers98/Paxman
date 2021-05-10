using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Vector2 _lastKnownPosition;
    private float _distanceToMove;
    private float _x;
    private float _y;
    
    protected Vector2 MovementDirection { get; private set; }

    protected virtual void SetMovementDirection(Vector2 movementDirection)
    {
        MovementDirection = movementDirection;
    }

    private void Update()
    {
        if (!IsStationary()) UpdatePosition();
    }

    private bool IsStationary()
    {
        if (MovementDirection == Vector2.zero) return true;
        if ((Vector2) transform.position == _lastKnownPosition)
        {
            MovementDirection = Vector2.zero;
            return true;
        }

        return false;
    }

    private void UpdatePosition()
    {
        _lastKnownPosition = transform.position;
        _distanceToMove = movementSpeed * Time.deltaTime;
        _x = _lastKnownPosition.x + MovementDirection.x * _distanceToMove;
        _y = _lastKnownPosition.y + MovementDirection.y * _distanceToMove;
        transform.position = new Vector2(_x, _y);
    }
}