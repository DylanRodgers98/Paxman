using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class GhostBehaviour : EdibleByPlayer
{
    private const float RespawnTime = 4.0f;
    
    public static event Action<int> OnGhostEaten;
    public static event Action OnGhostTouched;
    private static readonly Vector2[] AllDirections = {Vector2.down, Vector2.up, Vector2.left, Vector2.right};

    protected Vector2[] AvailableDirections;
    protected Vector2 MovementDirection;
    protected Vector2 LastKnownPosition;
    protected Vector2 PlayerPosition;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector2 scatterLocation;
    [SerializeField] private Vector2 spawnLocation;
    [SerializeField] private Transform playerTransform;
    private GhostMode _ghostMode;
    private IList<Vector2> _directionsToChooseFrom;
    private Vector2[] _desiredDirections;

    protected abstract void SetChaseDirection();

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Junction"))
        {
            AvailableDirections = other.GetComponent<JunctionBehaviour>().AvailableDirections;
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

    protected void SetRandomMovementDirection()
    {
        MovementDirection = AvailableDirections[
            AvailableDirections.Length == 1 ? 0 : Random.Range(0, AvailableDirections.Length)];
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
                SetScatterDirection();
                break;
            case GhostMode.Chase:
                SetChaseDirection();
                break;
            case GhostMode.Frightened:
                SetFrightenedDirection();
                break;
        }
    }

    private void SetScatterDirection()
    {
        if (scatterLocation.x > 0 && scatterLocation.y > 0)
        {
            _desiredDirections = new [] {Vector2.right, Vector2.up};
        }
        else if (scatterLocation.x < 0 && scatterLocation.y > 0)
        {
            _desiredDirections = new [] {Vector2.left, Vector2.up};
        }
        else if (scatterLocation.x > 0 && scatterLocation.y < 0)
        {
            _desiredDirections = new [] {Vector2.right, Vector2.down};
        }
        else if (scatterLocation.x < 0 && scatterLocation.y < 0)
        {
            _desiredDirections = new [] {Vector2.left, Vector2.down};
        }
        else
        {
            _desiredDirections = AllDirections;
        }

        foreach (Vector2 availableDirection in AvailableDirections)
        {
            if (_desiredDirections.Contains(availableDirection))
            {
                _directionsToChooseFrom.Add(availableDirection);
            }
        }

        if (_directionsToChooseFrom.Count > 0)
        {
            MovementDirection = _directionsToChooseFrom[
                _directionsToChooseFrom.Count == 1 ? 0 : Random.Range(0, _directionsToChooseFrom.Count)];
        }
        else
        {
            SetRandomMovementDirection();
        }
    }

    private void SetFrightenedDirection()
    {
        SetRandomMovementDirection();
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

        AvailableDirections = AllDirections;
        _directionsToChooseFrom = new List<Vector2>();
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        LastKnownPosition = transform.position;
        float distanceToMove = movementSpeed * Time.deltaTime;
        float x = LastKnownPosition.x + MovementDirection.x * distanceToMove;
        float y = LastKnownPosition.y + MovementDirection.y * distanceToMove;
        transform.position = new Vector2(x, y);
    }
}