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

    public static event Action<GhostMode> OnGhostModeChanged;
    private GhostMode _preFrightenedMode;
    private int _phaseNumber = 1;
    private float _phaseTimer;

    public static GhostManager Instance { get; private set; }
    public GhostMode PhaseMode { get; private set; }
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
        ghostBehaviour.Respawn();
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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
        _preFrightenedMode = PhaseMode;
        PhaseMode = GhostMode.Frightened;
        OnGhostModeChanged?.Invoke(PhaseMode);

        yield return new WaitForSeconds(frightenedTime);

        PhaseMode = _preFrightenedMode;
        OnGhostModeChanged?.Invoke(PhaseMode);
        ScoreOnEaten = baseScoreOnEaten;
    }

    private void IncreaseScoreOnEaten(int previousScore)
    {
        ScoreOnEaten = previousScore * scoreOnEatenIncreaseFactor;
    }

    private void Start()
    {
        PhaseMode = GhostMode.Scatter;
        OnGhostModeChanged?.Invoke(PhaseMode);
    }

    private void Update()
    {
        UpdatePhaseMode();
    }

    private void UpdatePhaseMode()
    {
        switch (PhaseMode)
        {
            case GhostMode.Scatter:
                _phaseTimer += Time.deltaTime;
                if ((_phaseNumber == 1 || _phaseNumber == 2) && _phaseTimer >= earlyScatterTime ||
                    (_phaseNumber == 3 || _phaseNumber == 4) && _phaseTimer >= lateScatterTime)
                {
                    PhaseMode = GhostMode.Chase;
                    OnGhostModeChanged?.Invoke(PhaseMode);
                    _phaseTimer = 0;
                }

                break;
            case GhostMode.Chase:
                if (_phaseNumber == 4) return;
                _phaseTimer += Time.deltaTime;
                if (_phaseTimer >= chaseTime)
                {
                    _phaseNumber++;
                    PhaseMode = GhostMode.Scatter;
                    OnGhostModeChanged?.Invoke(PhaseMode);
                    _phaseTimer = 0;
                }

                break;
        }
    }
}