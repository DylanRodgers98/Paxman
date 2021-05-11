﻿using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private float _distanceToMove;
    private float _x;
    private float _y;

    protected Transform PlayerTransform { get; private set; }
    protected Vector2 Direction { get; private set; }
    protected Vector2 LastKnownPosition { get; private set; }

    public virtual void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    private void Awake()
    {
        PlayerTransform = transform;
    }

    protected void Update()
    {
        if (Direction == Vector2.zero) return;
        LastKnownPosition = PlayerTransform.position;
        _distanceToMove = movementSpeed * Time.deltaTime;
        _x = LastKnownPosition.x + Direction.x * _distanceToMove;
        _y = LastKnownPosition.y + Direction.y * _distanceToMove;
        PlayerTransform.position = new Vector2(_x, _y);
    }
}