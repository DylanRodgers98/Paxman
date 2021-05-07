using UnityEngine;

public class JunctionBehaviour : MonoBehaviour
{
    [SerializeField] private Vector2[] availableDirections;

    public Vector2[] AvailableDirections => availableDirections;
}
