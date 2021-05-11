using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DotBehaviour : MonoBehaviour
{
    public static event Action<int> OnDotEaten;
    private static int _numberOfDots;
    [SerializeField] private int score;

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
            // TODO: implement something for when player eats all dots
            Time.timeScale = 0;
            Debug.Log("You ate all the dots!");
        }
    }
}