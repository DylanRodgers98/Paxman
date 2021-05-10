using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private float _distanceToMove;
    private float _x;
    private float _y;

    protected Vector2 MovementDirection { get; private set; }
    protected Vector2 LastKnownPosition { get; private set; }

    protected virtual void SetMovementDirection(Vector2 movementDirection)
    {
        MovementDirection = movementDirection;
    }

    private void Update()
    {
        if (MovementDirection == Vector2.zero) return;
        LastKnownPosition = transform.position;
        _distanceToMove = movementSpeed * Time.deltaTime;
        _x = LastKnownPosition.x + MovementDirection.x * _distanceToMove;
        _y = LastKnownPosition.y + MovementDirection.y * _distanceToMove;
        transform.position = new Vector2(_x, _y);
    }
}