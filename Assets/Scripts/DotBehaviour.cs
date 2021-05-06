using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DotBehaviour : MonoBehaviour
{
    [SerializeField] private int score;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // check isTrigger because player character has two colliders:
        // one that checks for collisions against walls (non-trigger),
        // and another that checks for collisions with dots (trigger).
        if (other.CompareTag("Player") && other.isTrigger)
        {
            other.GetComponent<PlayerController>().IncreaseScore(score);
            gameObject.SetActive(false);
        }
    }
}
