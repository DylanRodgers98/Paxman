using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBehaviour))]
public class PlayerLives : MonoBehaviour
{
    [SerializeField] private int initialLives = 3;
    [SerializeField] private float gameResetTime = 5.0f;
    private PlayerMovementBehaviour _playerMovementBehaviour;
    private GhostManager _ghostManager;
    private int _lives;
    private Vector2 _playerInitialPosition;

    private void Start()
    {
        _playerMovementBehaviour = GetComponent<PlayerMovementBehaviour>();
        _ghostManager = FindObjectOfType<GhostManager>();
        _lives = initialLives;
        _playerInitialPosition = transform.position;
    }

    private void OnEnable()
    {
        GhostBehaviour.OnGhostTouched += DecrementLives;
    }

    private void OnDisable()
    {
        GhostBehaviour.OnGhostTouched -= DecrementLives;
    }

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
            StartCoroutine(ResetLevel());
        }
    }

    private IEnumerator ResetLevel()
    {
        transform.position = _playerInitialPosition;
        _playerMovementBehaviour.Start();
        _ghostManager.ResetGhosts();

        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(gameResetTime);

        Time.timeScale = 1;
    }
}