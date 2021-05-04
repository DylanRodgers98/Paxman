using UnityEngine;

public class MoveOnTrigger : MonoBehaviour
{
    [SerializeField] private Transform destinationTransform;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.position = destinationTransform.position;
    }
}
