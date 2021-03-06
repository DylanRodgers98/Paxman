using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DotBehaviour : MonoBehaviour
{
    public static event Action<int> OnDotEaten;
    
    [SerializeField] private int score;
    private PacDotManager _pacDotManager;

    private void Start()
    {
        _pacDotManager = FindObjectOfType<PacDotManager>();
        _pacDotManager.AddDot(gameObject);
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
        _pacDotManager.DeactivateDot(gameObject);
    }
}