using UnityEngine;

public abstract class EdibleByPlayer : MonoBehaviour
{
    protected abstract void Eat();
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) Eat();
    }
}