using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class GhostBehaviour : MonoBehaviour
{
    public static event Action OnGhostTouched;
    public static event Action<int> OnGhostEaten;

    [SerializeField] private GhostMode initialGhostMode = GhostMode.Scatter;
    private GhostManager _ghostManager;
    private Transform _transform;
    private Vector2 _initialPosition;

    public GhostMode GhostMode { get; set; }

    private void Awake()
    {
        _transform = transform;
        _initialPosition = _transform.position;
    }

    private void OnEnable()
    {
        GhostManager.OnGhostModeChanged += SetGhostMode;
        GameManager.OnLevelReset += ResetGhost;
    }

    private void OnDisable()
    {
        GhostManager.OnGhostModeChanged -= SetGhostMode;
        GameManager.OnLevelReset -= ResetGhost;
    }

    private void SetGhostMode(GhostMode ghostMode)
    {
        GhostMode = ghostMode;
    }

    private void ResetGhost()
    {
        gameObject.SetActive(true);
        _transform.position = _initialPosition;
    }

    private void Start()
    {
        _ghostManager = FindObjectOfType<GhostManager>();
        GhostMode = initialGhostMode;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GhostMode != GhostMode.Frightened)
            {
                OnGhostTouched?.Invoke();
            }
            else
            {
                _ghostManager.KillThenRespawn(this);
                OnGhostEaten?.Invoke(_ghostManager.ScoreOnEaten);
            }
        }
    }
}