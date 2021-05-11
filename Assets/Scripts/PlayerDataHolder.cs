using System;
using UnityEngine;

public class PlayerDataHolder : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnLivesChanged;
    public static event Action OnPlayerLostLife;
    public static event Action OnPlayerDied;
    public static event Action<int> OnFinalScore;
    
    [SerializeField] private int initialLives = 3;
    private Transform _transform;
    private Vector2 _initialPosition;
    private int _lives;
    private int _score;

    private void Awake()
    {
        _transform = transform;
        _initialPosition = _transform.position;
    }

    private void OnEnable()
    {
        DotBehaviour.OnDotEaten += IncreaseScore;
        GhostBehaviour.OnGhostEaten += IncreaseScore;
        GhostBehaviour.OnGhostTouched += DecrementLives;
        LevelManager.OnLevelReset += ResetPosition;
    }

    private void OnDisable()
    {
        DotBehaviour.OnDotEaten -= IncreaseScore;
        GhostBehaviour.OnGhostEaten -= IncreaseScore;
        GhostBehaviour.OnGhostTouched -= DecrementLives;
        LevelManager.OnLevelReset -= ResetPosition;
    }

    private void IncreaseScore(int amount)
    {
        OnScoreChanged?.Invoke(_score += amount);
    }

    private void DecrementLives()
    {
        OnLivesChanged?.Invoke(--_lives);
        if (_lives == 0)
        {
            OnPlayerDied?.Invoke();
            OnFinalScore?.Invoke(_score);
        }
        else
        {
            OnPlayerLostLife?.Invoke();
        }
    }

    private void ResetPosition()
    {
        _transform.position = _initialPosition;
    }

    private void Start()
    {
        _lives = initialLives;
        OnScoreChanged?.Invoke(_score);
        OnLivesChanged?.Invoke(_lives);
    }
}