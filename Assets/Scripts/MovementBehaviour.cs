using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Transform _transform;
    private Vector2 _lastKnownPosition;
    private float _distanceToMove;
    private float _x;
    private float _y;

    protected Vector2 MovementDirection { get; private set; }

    protected virtual void SetMovementDirection(Vector2 movementDirection)
    {
        MovementDirection = movementDirection;
    }

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (MovementDirection == Vector2.zero) return;
        _lastKnownPosition = _transform.position;
        _distanceToMove = movementSpeed * Time.deltaTime;
        _x = _lastKnownPosition.x + MovementDirection.x * _distanceToMove;
        _y = _lastKnownPosition.y + MovementDirection.y * _distanceToMove;
        _transform.position = new Vector2(_x, _y);
    }
}