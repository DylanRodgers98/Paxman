using System;
using System.Collections;
using UnityEngine;

public class GhostBehaviour : EdibleByPlayer
{
    private const float RespawnTime = 4.0f;

    public static event Action<int> OnGhostEaten;
    public static event Action OnGhostTouched;

    [SerializeField] private Transform spawnLocationTransform;
    [SerializeField] private Transform scatterLocationTransform;
    private GhostMode _ghostMode;

    protected override void Eat()
    {
        if (_ghostMode == GhostMode.Frightened)
        {
            OnGhostEaten?.Invoke(GhostManager.Instance.ScoreOnEaten);
            StartCoroutine(DieThenRespawn());
        }
        else
        {
            OnGhostTouched?.Invoke();
        }
    }

    private IEnumerator DieThenRespawn()
    {
        gameObject.SetActive(false);

        yield return new WaitForSeconds(RespawnTime);

        transform.position = spawnLocationTransform.position;
        _ghostMode = GhostManager.Instance.PhaseMode;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        GhostManager.Instance.OnGhostModeChanged += SetGhostMode;
    }

    private void OnDisable()
    {
        GhostManager.Instance.OnGhostModeChanged -= SetGhostMode;
    }

    private void SetGhostMode(GhostMode ghostMode)
    {
        _ghostMode = ghostMode;
    }
}