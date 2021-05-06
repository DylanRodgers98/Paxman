using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DotBehaviour : EdibleByPlayer
{
    public static event Action<int> OnDotEaten;
    [SerializeField] private int score;

    protected override void Eat()
    {
        OnDotEaten?.Invoke(score);
        gameObject.SetActive(false);
    }
}