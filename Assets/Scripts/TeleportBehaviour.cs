using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TeleportBehaviour : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.position = teleportDestination.position;
    }
}
