using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Transform _transform;
    private Vector2 _lastKnownPosition;
    private float _distanceToMove;
    private float _x;
    private float _y;

    protected Vector2 Direction { get; private set; }

    public virtual void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (Direction == Vector2.zero) return;
        _lastKnownPosition = _transform.position;
        _distanceToMove = movementSpeed * Time.deltaTime;
        _x = _lastKnownPosition.x + Direction.x * _distanceToMove;
        _y = _lastKnownPosition.y + Direction.y * _distanceToMove;
        _transform.position = new Vector2(_x, _y);
    }
}