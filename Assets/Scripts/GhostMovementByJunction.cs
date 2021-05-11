using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MovementBehaviour), typeof(GhostBehaviour))]
public class GhostMovementByJunction : MonoBehaviour
{
    private static readonly Vector2[] UpAndLeft = {Vector2.up, Vector2.left};
    private static readonly Vector2[] UpAndRight = {Vector2.up, Vector2.right};
    private static readonly Vector2[] DownAndLeft = {Vector2.down, Vector2.left};
    private static readonly Vector2[] DownAndRight = {Vector2.down, Vector2.right};
    
    [SerializeField] private Vector2 scatterLocation;
    [SerializeField] private Transform playerTransform;
    private MovementBehaviour _movementBehaviour;
    private GhostBehaviour _ghostBehaviour;
    private Vector2 _currentPosition;
    private Vector2[] _desiredDirections;
    private Vector2[] _availableDirections;
    private Vector2[] _directionsToChooseFrom;

    private void Start()
    {
        _movementBehaviour = GetComponent<MovementBehaviour>();
        _ghostBehaviour = GetComponent<GhostBehaviour>();
        if (!playerTransform)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Junction"))
        {
            _availableDirections = other.GetComponent<JunctionBehaviour>().AvailableDirections;
            SetDirection();
        }
    }

    private void SetDirection()
    {
        switch (_ghostBehaviour.GhostMode)
        {
            case GhostMode.Scatter:
                SetDirection(scatterLocation);
                break;
            case GhostMode.Chase:
                SetDirection(playerTransform.position);
                break;
            case GhostMode.Frightened:
                SetRandomDirection();
                break;
        }
    }
    
    private void SetDirection(Vector2 targetLocation)
    {
        _currentPosition = transform.position;

        if (targetLocation.x >= _currentPosition.x && targetLocation.y >= _currentPosition.y)
        {
            _desiredDirections = UpAndRight;
        }
        else if (targetLocation.x < _currentPosition.x && targetLocation.y >= _currentPosition.y)
        {
            _desiredDirections = UpAndLeft;
        }
        else if (targetLocation.x >= _currentPosition.x && targetLocation.y < _currentPosition.y)
        {
            _desiredDirections = DownAndRight;
        }
        else if (targetLocation.x < _currentPosition.x && targetLocation.y < _currentPosition.y)
        {
            _desiredDirections = DownAndLeft;
        }
        
        _directionsToChooseFrom = _availableDirections.Where(_desiredDirections.Contains).ToArray();

        if (_directionsToChooseFrom.Length > 0)
        {
            _movementBehaviour.SetDirection(ChooseRandomVector(_directionsToChooseFrom));
        }
        else
        {
            SetRandomDirection();
        }
    }

    private void SetRandomDirection()
    {
        _movementBehaviour.SetDirection(ChooseRandomVector(_availableDirections));
    }

    private static Vector2 ChooseRandomVector(Vector2[] vectors)
    {
        return vectors[vectors.Length == 1 ? 0 : Random.Range(0, vectors.Length)];
    }
}
