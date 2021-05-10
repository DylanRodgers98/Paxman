using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class GhostBehaviour : MonoBehaviour
{
    public static event Action OnGhostTouched;
    public static event Action<int> OnGhostEaten;

    [SerializeField] private Vector2 respawnLocation;
    [SerializeField] private GhostMode initialGhostMode = GhostMode.Scatter;
    
    public GhostMode GhostMode { get; private set; }

    public void Respawn()
    {
        transform.position = respawnLocation;
        GhostMode = GhostManager.Instance.PhaseMode;
    }

    private void OnEnable()
    {
        GhostManager.OnGhostModeChanged += SetGhostMode;
    }

    private void OnDisable()
    {
        GhostManager.OnGhostModeChanged -= SetGhostMode;
    }

    private void SetGhostMode(GhostMode ghostMode)
    {
        GhostMode = ghostMode;
    }

    private void Start()
    {
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
                GhostManager.Instance.KillThenRespawn(this);
                OnGhostEaten?.Invoke(GhostManager.Instance.ScoreOnEaten);
            }
        }
    }
}