using UnityEngine;

public abstract class EdibleByPlayer : MonoBehaviour
{
    protected abstract void Eat();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // check isTrigger because player character has two colliders:
        // one that checks for collisions against walls (non-trigger),
        // and another that checks for collisions with dots (trigger).
        if (other.CompareTag("Player") && other.isTrigger) Eat();
    }
}