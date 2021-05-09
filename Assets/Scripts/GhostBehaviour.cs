using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class GhostBehaviour : MonoBehaviour
{
    public static event Action OnGhostTouched;
    private static readonly Vector2[] UpAndLeft = {Vector2.up, Vector2.left};
    private static readonly Vector2[] UpAndRight = {Vector2.up, Vector2.right};
    private static readonly Vector2[] DownAndLeft = {Vector2.down, Vector2.left};
    private static readonly Vector2[] DownAndRight = {Vector2.down, Vector2.right};
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector2 scatterLocation;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GhostMode initialGhostMode = GhostMode.Scatter;
    private Vector2[] _desiredDirections;
    private Vector2[] _availableDirections;
    private Vector2[] _directionsToChooseFrom;
    private Vector2 _movementDirection;
    private Vector2 _lastKnownPosition;
    
    public GhostMode GhostMode { get; set; }

    private void OnEnable()
    {
        GhostManager.OnGhostModeChanged += SetGhostMode;
    }

    private void OnDisable()
    {
        GhostManager.OnGhostModeChanged -= SetGhostMode;
    }

    private void SetGhostMode(GhostMode ghostMode)
    {
        GhostMode = ghostMode;
    }

    private void Start()
    {
        if (!playerTransform)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        GhostMode = initialGhostMode;
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        _lastKnownPosition = transform.position;
        float distanceToMove = movementSpeed * Time.deltaTime;
        float x = _lastKnownPosition.x + _movementDirection.x * distanceToMove;
        float y = _lastKnownPosition.y + _movementDirection.y * distanceToMove;
        transform.position = new Vector2(x, y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (GhostMode != GhostMode.Frightened && other.gameObject.CompareTag("Player"))
        {
            OnGhostTouched?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Junction"))
        {
            _availableDirections = other.GetComponent<JunctionBehaviour>().AvailableDirections;
            transform.position = other.transform.position;
            SetMovementDirection();
        }
    }

    private void SetMovementDirection()
    {
        switch (GhostMode)
        {
            case GhostMode.Scatter:
                SetMovementDirection(scatterLocation);
                break;
            case GhostMode.Chase:
                SetMovementDirection(playerTransform.position);
                break;
            case GhostMode.Frightened:
                SetRandomMovementDirection();
                break;
        }
    }

    private void SetMovementDirection(Vector2 targetLocation)
    {
        _lastKnownPosition = transform.position;
        
        if (targetLocation.x >= _lastKnownPosition.x && targetLocation.y >= _lastKnownPosition.y)
        {
            _desiredDirections = UpAndRight;
        }
        else if (targetLocation.x < _lastKnownPosition.x && targetLocation.y >= _lastKnownPosition.y)
        {
            _desiredDirections = UpAndLeft;
        }
        else if (targetLocation.x >= _lastKnownPosition.x && targetLocation.y < _lastKnownPosition.y)
        {
            _desiredDirections = DownAndRight;
        }
        else if (targetLocation.x < _lastKnownPosition.x && targetLocation.y < _lastKnownPosition.y)
        {
            _desiredDirections = DownAndLeft;
        }

        _directionsToChooseFrom = _availableDirections.Where(_desiredDirections.Contains).ToArray();

        if (_directionsToChooseFrom.Length > 0)
        {
            _movementDirection = _directionsToChooseFrom[
                _directionsToChooseFrom.Length == 1 ? 0 : Random.Range(0, _directionsToChooseFrom.Length)];
        }
        else
        {
            SetRandomMovementDirection();
        }
    }

    private void SetRandomMovementDirection()
    {
        _movementDirection = _availableDirections[
            _availableDirections.Length == 1 ? 0 : Random.Range(0, _availableDirections.Length)];
    }
}