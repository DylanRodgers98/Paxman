using System;
using System.Collections;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] private float frightenedTime = 6.0f;
    [SerializeField] private float chaseTime = 20.0f;
    [SerializeField] private float earlyScatterTime = 7.0f;
    [SerializeField] private float lateScatterTime = 5.0f;
    [SerializeField] private int baseScoreOnEaten = 200;
    [SerializeField] private int scoreOnEatenIncreaseFactor = 2;
    [SerializeField] private float respawnTime = 4.0f;
    [SerializeField] private Transform respawnJunctionTransform;

    public static event Action<GhostMode> OnGhostModeChanged;
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
        ghostGameObject.transform.position = respawnJunctionTransform.position;
        ghostBehaviour.GhostMode = _phaseMode;
    }

    private void OnEnable()
    {
        PowerPelletBehaviour.OnPowerPelletEaten += FrightenGhosts;
        GhostBehaviour.OnGhostEaten += IncreaseScoreOnEaten;
        GameManager.OnLevelStart += InitialisePhaseTimer;
        GameManager.OnLevelReset += DisablePhaseTimer;
        PlayerDataHolder.OnPlayerDied += DisablePhaseTimer;
    }

    private void OnDisable()
    {
        PowerPelletBehaviour.OnPowerPelletEaten -= FrightenGhosts;
        GhostBehaviour.OnGhostEaten -= IncreaseScoreOnEaten;
        GameManager.OnLevelStart -= InitialisePhaseTimer;
        GameManager.OnLevelReset -= DisablePhaseTimer;
        PlayerDataHolder.OnPlayerDied -= DisablePhaseTimer;
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

    private void InitialisePhaseTimer()
    {
        _phaseNumber = 1;
        _phaseTimer = 0;
        SetPhaseMode(GhostMode.Scatter);
    }

    private void DisablePhaseTimer()
    {
        SetPhaseMode(GhostMode.Disabled);
    }

    private void SetPhaseMode(GhostMode phaseMode)
    {
        _phaseMode = phaseMode;
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