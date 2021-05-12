using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action OnLevelStart;
    public static event Action OnLevelReset;
    
    [SerializeField] private float startCountdown;

    private void OnEnable()
    {
        PlayerDataHolder.OnPlayerLostLife += ResetLevel;
        DotBehaviour.OnAllDotsEaten += ResetLevel;
    }

    private void OnDisable()
    {
        PlayerDataHolder.OnPlayerLostLife -= ResetLevel;
        DotBehaviour.OnAllDotsEaten += ResetLevel;
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
