using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDied;
    [SerializeField] private float movementSpeed;
    private Vector2 _movementDirection = Vector2.left;
    private Vector3 _lastKnownPosition;
    private int _score;
    private int _lives;

    private void OnEnable()
    {
        DotBehaviour.OnDotEaten += IncreaseScore;
        EatGhostBehaviour.OnGhostEaten += IncreaseScore;
        GhostBehaviour.OnGhostTouched += DecrementLives;
    }

    private void OnDisable()
    {
        DotBehaviour.OnDotEaten -= IncreaseScore;
        EatGhostBehaviour.OnGhostEaten -= IncreaseScore;
        GhostBehaviour.OnGhostTouched -= DecrementLives;
    }

    private void IncreaseScore(int amount) => _score += amount;

    private void DecrementLives()
    {
        _lives--;
        if (_lives == 0) OnPlayerDied?.Invoke();
    }

    private void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movementDirection = context.ReadValue<Vector2>();
            SetScale();
            SetRotation();
        }
    }

    private void SetScale()
    {
        transform.localScale = _movementDirection == Vector2.right
            ? new Vector3(-1, 1, 1) // flip sprite across x axis if facing right direction
            : Vector3.one; // keep sprite scale normal for all other directions
    }

    private void SetRotation()
    {
        if (_movementDirection == Vector2.left || _movementDirection == Vector2.right)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (_movementDirection == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (_movementDirection == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    private void Update()
    {
        if (!IsStationary()) UpdatePosition();
    }

    private bool IsStationary()
    {
        if (_movementDirection == Vector2.zero) return true;
        if (transform.position == _lastKnownPosition)
        {
            _movementDirection = Vector2.zero;
            return true;
        }

        return false;
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