using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] private GameObject[] ghostGameObjects;
    [SerializeField] private float frightenedTime = 6.0f;
    [SerializeField] private float chaseTime = 20.0f;
    [SerializeField] private float earlyScatterTime = 7.0f;
    [SerializeField] private float lateScatterTime = 5.0f;
    [SerializeField] private int baseScoreOnEaten = 200;
    [SerializeField] private int scoreOnEatenIncreaseFactor = 2;
    [SerializeField] private float respawnTime = 4.0f;
    [SerializeField] private Vector2 respawnPosition;

    public static event Action<GhostMode> OnGhostModeChanged;
    private Tuple<GameObject, Vector2>[] _ghostInitialPositions;
    private GhostMode _preFrightenedMode;
    private int _phaseNumber = 1;
    private float _phaseTimer;
    private GhostMode _phaseMode;

    public int ScoreOnEaten { get; private set; }

    public void KillThenRespawn(GhostBehaviour ghostBehaviour)
    {
        StartCoroutine(DoKillThenRespawn(ghostBehaviour));
    }

    private IEnumerator DoKillThenRespawn(GhostBehaviour ghostBehaviour)
    {
        GameObject ghostGameObject = ghostBehaviour.gameObject;
        ghostGameObject.SetActive(false);

        yield return new WaitForSeconds(respawnTime);

        ghostGameObject.SetActive(true);
        ghostGameObject.transform.position = respawnPosition;
        ghostBehaviour.GhostMode = _phaseMode;
    }

    public void ResetGhosts()
    {
        _phaseNumber = 1;
        _phaseTimer = 0;

        foreach ((GameObject ghostTransform, Vector2 ghostInitialPosition) in _ghostInitialPositions)
        {
            ghostTransform.SetActive(true);
            ghostTransform.transform.position = ghostInitialPosition;
        }

        SetPhaseModeToScatter();
    }

    private void OnEnable()
    {
        PowerPelletBehaviour.OnPowerPelletEaten += FrightenGhosts;
        GhostBehaviour.OnGhostEaten += IncreaseScoreOnEaten;
    }

    private void OnDisable()
    {
        PowerPelletBehaviour.OnPowerPelletEaten -= FrightenGhosts;
        GhostBehaviour.OnGhostEaten -= IncreaseScoreOnEaten;
    }

    private void FrightenGhosts()
    {
        StartCoroutine(DoFrightenGhosts());
    }

    private IEnumerator DoFrightenGhosts()
    {
        _preFrightenedMode = _phaseMode;
        _phaseMode = GhostMode.Frightened;
        OnGhostModeChanged?.Invoke(_phaseMode);

        yield return new WaitForSeconds(frightenedTime);

        _phaseMode = _preFrightenedMode;
        OnGhostModeChanged?.Invoke(_phaseMode);
        ScoreOnEaten = baseScoreOnEaten;
    }

    private void IncreaseScoreOnEaten(int previousScore)
    {
        ScoreOnEaten = previousScore * scoreOnEatenIncreaseFactor;
    }

    private void Start()
    {
        if (ghostGameObjects == null || ghostGameObjects.Length == 0)
        {
            ghostGameObjects = GameObject.FindGameObjectsWithTag("Ghost");
        }

        _ghostInitialPositions = new Tuple<GameObject, Vector2>[ghostGameObjects.Length];
        for (int i = 0; i < ghostGameObjects.Length; i++)
        {
            GameObject ghostGameObject = ghostGameObjects[i];
            Vector2 ghostInitialPosition = ghostGameObject.transform.position;
            _ghostInitialPositions[i] = Tuple.Create(ghostGameObject, ghostInitialPosition);
        }

        SetPhaseModeToScatter();
    }

    private void SetPhaseModeToScatter()
    {
        _phaseMode = GhostMode.Scatter;
        OnGhostModeChanged?.Invoke(_phaseMode);
    }

    private void Update()
    {
        UpdatePhaseMode();
    }

    private void UpdatePhaseMode()
    {
        switch (_phaseMode)
        {
            case GhostMode.Scatter:
                _phaseTimer += Time.deltaTime;
                if ((_phaseNumber == 1 || _phaseNumber == 2) && _phaseTimer >= earlyScatterTime ||
                    (_phaseNumber == 3 || _phaseNumber == 4) && _phaseTimer >= lateScatterTime)
                {
                    _phaseMode = GhostMode.Chase;
                    OnGhostModeChanged?.Invoke(_phaseMode);
                    _phaseTimer = 0;
                }

                break;
            case GhostMode.Chase:
                if (_phaseNumber == 4) return;
                _phaseTimer += Time.deltaTime;
                if (_phaseTimer >= chaseTime)
                {
                    _phaseNumber++;
                    _phaseMode = GhostMode.Scatter;
                    OnGhostModeChanged?.Invoke(_phaseMode);
                    _phaseTimer = 0;
                }

                break;
        }
    }
}