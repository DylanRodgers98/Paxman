using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnLivesChanged;
    public static event Action OnPlayerLostLife;
    public static event Action<int> OnPlayerDied;
    
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
        GameManager.OnLevelReset += ResetPosition;
    }

    private void OnDisable()
    {
        DotBehaviour.OnDotEaten -= IncreaseScore;
        GhostBehaviour.OnGhostEaten -= IncreaseScore;
        GhostBehaviour.OnGhostTouched -= DecrementLives;
        GameManager.OnLevelReset -= ResetPosition;
    }

    private void IncreaseScore(int amount)
    {
        _score += amount;
        OnScoreChanged?.Invoke(_score);
    }

    private void DecrementLives()
    {
        --_lives;
        OnLivesChanged?.Invoke(_lives);
        if (_lives == 0)
        {
            // TODO: implement something for when player loses all lives
            Time.timeScale = 0;
            Debug.Log("You died!");
            OnPlayerDied?.Invoke(_score);
        }
        else
        {
            OnPlayerLostLife?.Invoke();
        }
    }

    private void ResetPosition() => _transform.position = _initialPosition;

    private void Start()
    {
        _lives = initialLives;
        OnScoreChanged?.Invoke(_score);
        OnLivesChanged?.Invoke(_lives);
    }
}