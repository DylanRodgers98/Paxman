using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnLevelStart;
    public static event Action OnLevelReset;
    
    [SerializeField] private float startCountdown;

    private void OnEnable()
    {
        PlayerController.OnPlayerLostLife += ResetLevel;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerLostLife -= ResetLevel;
    }

    private void ResetLevel()
    {
        OnLevelReset?.Invoke();
        StartCoroutine(CountdownThenStart());
    }
    
    private void Start()
    {
        StartCoroutine(CountdownThenStart());
    }

    private IEnumerator CountdownThenStart()
    {
        yield return new WaitForSecondsRealtime(startCountdown);
        OnLevelStart?.Invoke();
    }
}
