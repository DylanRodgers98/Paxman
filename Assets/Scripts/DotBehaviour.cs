using System;
using UnityEngine;

public class DotBehaviour : EdibleByPlayer
{
    public static event Action<int> OnDotEaten;
    [SerializeField] private int score;
    private PacDotManager _pacDotManager;

    protected override void Eat()
    {
        OnDotEaten?.Invoke(score);
        gameObject.SetActive(false);
        _pacDotManager.DecrementNumberOfDots();
    }

    private void Start()
    {
        _pacDotManager = FindObjectOfType<PacDotManager>();
        _pacDotManager.IncrementNumberOfDots();
    }
}