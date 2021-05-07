using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GhostBehaviour), typeof(Collider2D), typeof(Rigidbody2D))]
public class EatGhostBehaviour : MonoBehaviour
{
    public static event Action<int> OnGhostEaten;
    
    [SerializeField] private float respawnTime = 4.0f;
    [SerializeField] private Vector2 respawnLocation;
    private GhostBehaviour _ghostBehaviour;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_ghostBehaviour.GhostMode == GhostMode.Frightened && other.gameObject.CompareTag("Player"))
        {
            OnGhostEaten?.Invoke(GhostManager.Instance.ScoreOnEaten);
            StartCoroutine(DieThenRespawn());
        }
    }

    private IEnumerator DieThenRespawn()
    {
        gameObject.SetActive(false);

        yield return new WaitForSeconds(respawnTime);

        transform.position = respawnLocation;
        _ghostBehaviour.GhostMode = GhostManager.Instance.PhaseMode;
        gameObject.SetActive(true);
    }

    private void Start()
    {
        _ghostBehaviour = GetComponent<GhostBehaviour>();
    }
}
