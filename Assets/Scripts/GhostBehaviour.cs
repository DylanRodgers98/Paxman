using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostBehaviour : EdibleByPlayer
{
    private const float RespawnTime = 4.0f;
    
    public static event Action<int> OnGhostEaten;
    public static event Action OnGhostTouched;
    private static readonly Vector2[] AllDirections = {Vector2.down, Vector2.up, Vector2.left, Vector2.right};
    private static readonly Vector2[] UpAndLeft = {Vector2.up, Vector2.left};
    private static readonly Vector2[] UpAndRight = {Vector2.up, Vector2.right};
    private static readonly Vector2[] DownAndLeft = {Vector2.down, Vector2.left};
    private static readonly Vector2[] DownAndRight = {Vector2.down, Vector2.right};
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector2 scatterLocation;
    [SerializeField] private Vector2 spawnLocation;
    [SerializeField] private Transform playerTransform;
    private GhostMode _ghostMode;
    private Vector2[] _desiredDirections;
    private Vector2[] _availableDirections;
    private Vector2[] _directionsToChooseFrom;
    private Vector2 _movementDirection;
    private Vector2 _lastKnownPosition;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Junction"))
        {
            _availableDirections = other.GetComponent<JunctionBehaviour>().AvailableDirections;
            transform.position = other.transform.position;
            SetMovementDirection();
        }
    }

    protected override void Eat()
    {
        if (_ghostMode == GhostMode.Frightened)
        {
            OnGhostEaten?.Invoke(GhostManager.Instance.ScoreOnEaten);
            StartCoroutine(DieThenRespawn());
        }
        else
        {
            OnGhostTouched?.Invoke();
        }
    }

    private IEnumerator DieThenRespawn()
    {
        gameObject.SetActive(false);

        yield return new WaitForSeconds(RespawnTime);

        transform.position = spawnLocation;
        _ghostMode = GhostManager.Instance.PhaseMode;
        gameObject.SetActive(true);
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
        if (targetLocation.x >= 0 && targetLocation.y >= 0)
        {
            _desiredDirections = UpAndRight;
        }
        else if (targetLocation.x < 0 && targetLocation.y >= 0)
        {
            _desiredDirections = UpAndLeft;
        }
        else if (targetLocation.x >= 0 && targetLocation.y < 0)
        {
            _desiredDirections = DownAndRight;
        }
        else if (targetLocation.x < 0 && targetLocation.y < 0)
        {
            _desiredDirections = DownAndLeft;
        }
        else
        {
            _desiredDirections = AllDirections;
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

        _availableDirections = AllDirections;
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
}