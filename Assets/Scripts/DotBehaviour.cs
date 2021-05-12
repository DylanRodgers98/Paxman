using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DotBehaviour : MonoBehaviour
{
    public static event Action<int> OnDotEaten;
    public static event Action OnAllDotsEaten;
    private static int _numberOfDots;
    
    [SerializeField] private int score;

    private void OnEnable()
    {
        OnAllDotsEaten += ReactivateDot;
    }

    private void OnDisable()
    {
        OnAllDotsEaten -= ReactivateDot;
    }

    private void ReactivateDot()
    {
        gameObject.SetActive(true);
        IncrementNumberOfDots();
    }

    private void Start()
    {
        IncrementNumberOfDots();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // check isTrigger because player character has two colliders:
        // one that checks for collisions against walls (non-trigger),
        // and another that checks for collisions with dots (trigger).
        if (other.CompareTag("Player") && other.isTrigger) Eat();
    }

    protected virtual void Eat()
    {
        OnDotEaten?.Invoke(score);
        gameObject.SetActive(false);
        DecrementNumberOfDots();
    }

    private static void IncrementNumberOfDots()
    {
        _numberOfDots++;
    }

    private static void DecrementNumberOfDots()
    {
        if (--_numberOfDots == 0)
        {
            OnAllDotsEaten?.Invoke();
        }
    }
}