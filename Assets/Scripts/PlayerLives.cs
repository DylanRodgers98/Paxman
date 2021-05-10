using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBehaviour))]
public class PlayerLives : MonoBehaviour
{
    [SerializeField] private int initialLives = 3;
    [SerializeField] private float gameResetTime = 5.0f;
    [SerializeField] private GameObject[] ghostGameObjects;
    private PlayerMovementBehaviour _playerMovementBehaviour;
    private int _lives;
    private Vector2 _playerInitialPosition;
    private IDictionary<Transform, Vector2> _ghostInitialPositions;

    private void Start()
    {
        _playerMovementBehaviour = GetComponent<PlayerMovementBehaviour>();
        _lives = initialLives;
        _playerInitialPosition = transform.position;
        if (ghostGameObjects == null || ghostGameObjects.Length == 0)
        {
            ghostGameObjects = GameObject.FindGameObjectsWithTag("Ghost");
        }

        _ghostInitialPositions = new Dictionary<Transform, Vector2>();
        foreach (GameObject ghostGameObject in ghostGameObjects)
        {
            _ghostInitialPositions[ghostGameObject.transform] = ghostGameObject.transform.position;
        }
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
            StartCoroutine(ResetGame());
        }
    }

    private IEnumerator ResetGame()
    {
        transform.position = _playerInitialPosition;
        _playerMovementBehaviour.Start();

        foreach (var kvp in _ghostInitialPositions)
        {
            Transform ghostTransform = kvp.Key;
            Vector2 initialPosition = kvp.Value;
            ghostTransform.gameObject.SetActive(true);
            ghostTransform.position = initialPosition;
        }

        GhostManager.Instance.ResetPhases();
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(gameResetTime);

        Time.timeScale = 1;
    }
}