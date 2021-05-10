using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(GhostBehaviour))]
public class GhostMovementBehaviour : MovementBehaviour
{
    private static readonly Vector2[] UpAndLeft = {Vector2.up, Vector2.left};
    private static readonly Vector2[] UpAndRight = {Vector2.up, Vector2.right};
    private static readonly Vector2[] DownAndLeft = {Vector2.down, Vector2.left};
    private static readonly Vector2[] DownAndRight = {Vector2.down, Vector2.right};
    
    [SerializeField] private Vector2 scatterLocation;
    [SerializeField] private Transform playerTransform;
    private GhostBehaviour _ghostBehaviour;
    private Vector2 _currentPosition;
    private Vector2[] _desiredDirections;
    private Vector2[] _availableDirections;
    private Vector2[] _directionsToChooseFrom;

    private void Start()
    {
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
            transform.position = other.transform.position;
            SetMovementDirection();
        }
    }

    private void SetMovementDirection()
    {
        switch (_ghostBehaviour.GhostMode)
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
    
    protected override void SetMovementDirection(Vector2 targetLocation)
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
            base.SetMovementDirection(_directionsToChooseFrom[
                _directionsToChooseFrom.Length == 1 ? 0 : Random.Range(0, _directionsToChooseFrom.Length)]);
        }
        else
        {
            SetRandomMovementDirection();
        }
    }

    private void SetRandomMovementDirection()
    {
        base.SetMovementDirection(_availableDirections[
            _availableDirections.Length == 1 ? 0 : Random.Range(0, _availableDirections.Length)]);
    }
}
