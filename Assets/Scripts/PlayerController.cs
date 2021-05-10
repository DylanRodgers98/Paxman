using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDied;
    [SerializeField] private int initialLives = 3;
    private int _score;
    private int _lives;

    private void OnEnable()
    {
        DotBehaviour.OnDotEaten += IncreaseScore;
        GhostBehaviour.OnGhostEaten += IncreaseScore;
        GhostBehaviour.OnGhostTouched += DecrementLives;
    }

    private void OnDisable()
    {
        DotBehaviour.OnDotEaten -= IncreaseScore;
        GhostBehaviour.OnGhostEaten -= IncreaseScore;
        GhostBehaviour.OnGhostTouched -= DecrementLives;
    }

    private void IncreaseScore(int amount) => _score += amount;

    private void DecrementLives()
    {
        _lives--;
        if (_lives == 0) OnPlayerDied?.Invoke();
    }

    private void Start()
    {
        _lives = initialLives;
    }
}