using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStart; 
    [SerializeField] private float startCountdown;
    
    private void Awake()
    {
        StartCoroutine(CountdownThenStart());
    }

    private IEnumerator CountdownThenStart()
    {
        yield return new WaitForSecondsRealtime(startCountdown);
        OnGameStart?.Invoke();
    }
}
