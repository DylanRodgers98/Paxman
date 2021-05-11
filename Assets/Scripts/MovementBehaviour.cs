using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private float _distanceToMove;
    private float _x;
    private float _y;
    
    public IList<Tuple<float, Vector2>> DirectionHistory { get; private set; }
    protected Transform PlayerTransform { get; private set; }
    protected bool IsMovementEnabled { get; private set; }
    protected Vector2 Direction { get; private set; }
    protected Vector2 LastKnownPosition { get; private set; }

    public virtual void SetDirection(Vector2 direction)
    {
        Direction = direction;
        DirectionHistory.Add(Tuple.Create(Time.timeSinceLevelLoad, direction));
    }

    private void Awake()
    {
        DirectionHistory = new List<Tuple<float, Vector2>>();
        PlayerTransform = transform;
    }

    protected virtual void OnEnable()
    {
        LevelManager.OnLevelStart += EnableMovement;
        LevelManager.OnLevelReset += DisableMovement;
        PlayerDataHolder.OnPlayerDied += DisableMovement;
    }

    protected virtual void OnDisable()
    {
        LevelManager.OnLevelStart -= EnableMovement;
        LevelManager.OnLevelReset -= DisableMovement;
        PlayerDataHolder.OnPlayerDied -= DisableMovement;
    }

    private void EnableMovement() => IsMovementEnabled = true;

    private void DisableMovement() => IsMovementEnabled = false;

    protected void Update()
    {
        if (!IsMovementEnabled || Direction == Vector2.zero) return;
        LastKnownPosition = PlayerTransform.position;
        _distanceToMove = movementSpeed * Time.deltaTime;
        _x = LastKnownPosition.x + Direction.x * _distanceToMove;
        _y = LastKnownPosition.y + Direction.y * _distanceToMove;
        PlayerTransform.position = new Vector2(_x, _y);
    }
}