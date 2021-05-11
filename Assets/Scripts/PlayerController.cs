using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerLostLife;
    [SerializeField] private int initialLives = 3;
    private Transform _transform;
    private Vector2 _initialPosition;
    private int _lives;
    private int _score;

    private void Awake()
    {
        _transform = transform;
        _initialPosition = _transform.position;
        _lives = initialLives;
    }

    private void OnEnable()
    {
        DotBehaviour.OnDotEaten += IncreaseScore;
        GhostBehaviour.OnGhostEaten += IncreaseScore;
        GhostBehaviour.OnGhostTouched += DecrementLives;
        GameManager.OnLevelReset += ResetPosition;
    }

    private void OnDisable()
    {
        DotBehaviour.OnDotEaten -= IncreaseScore;
        GhostBehaviour.OnGhostEaten -= IncreaseScore;
        GhostBehaviour.OnGhostTouched -= DecrementLives;
        GameManager.OnLevelReset -= ResetPosition;
    }

    private void IncreaseScore(int amount) => _score += amount;

    private void DecrementLives()
    {
        if (--_lives == 0)
        {
            // TODO: implement something for when player loses all lives
            Time.timeScale = 0;
            Debug.Log("You died!");
        }
        else
        {
            OnPlayerLostLife?.Invoke();
        }
    }

    private void ResetPosition() => _transform.position = _initialPosition;
}