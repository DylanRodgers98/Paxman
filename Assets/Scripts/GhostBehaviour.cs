using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class GhostBehaviour : MonoBehaviour
{
    public static event Action OnGhostTouched;
    public static event Action<int> OnGhostEaten;
    private static readonly Vector2[] UpAndLeft = {Vector2.up, Vector2.left};
    private static readonly Vector2[] UpAndRight = {Vector2.up, Vector2.right};
    private static readonly Vector2[] DownAndLeft = {Vector2.down, Vector2.left};
    private static readonly Vector2[] DownAndRight = {Vector2.down, Vector2.right};
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector2 scatterLocation;
    [SerializeField] private Vector2 respawnLocation;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GhostMode initialGhostMode = GhostMode.Scatter;
    private Vector2[] _desiredDirections;
    private Vector2[] _availableDirections;
    private Vector2[] _directionsToChooseFrom;
    private Vector2 _movementDirection;
    private Vector2 _lastKnownPosition;
    private GhostMode _ghostMode;

    public void Respawn()
    {
        transform.position = respawnLocation;
        _movementDirection = Vector2.zero;
        _ghostMode = GhostManager.Instance.PhaseMode;
    }

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
        _ghostMode = ghostMode;
    }

    private void Start()
    {
        if (!playerTransform)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        _ghostMode = initialGhostMode;
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
        if (other.gameObject.CompareTag("Player"))
        {
            if (_ghostMode != GhostMode.Frightened)
            {
                OnGhostTouched?.Invoke();
            }
            else
            {
                GhostManager.Instance.KillThenRespawn(this);
                OnGhostEaten?.Invoke(GhostManager.Instance.ScoreOnEaten);
            }
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
        switch (_ghostMode)
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