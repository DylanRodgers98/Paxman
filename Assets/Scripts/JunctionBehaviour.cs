using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JunctionBehaviour : MonoBehaviour
{
    [Header("Available Directions")]
    [SerializeField] private bool up;
    [SerializeField] private bool down;
    [SerializeField] private bool left;
    [SerializeField] private bool right;
    
    private Vector2[] _availableDirections;

    public Vector2[] AvailableDirections
    {
        get
        {
            if (_availableDirections == null)
            {
                IList<Vector2> directions = new List<Vector2>();
                if (up) directions.Add(Vector2.up);
                if (down) directions.Add(Vector2.down);
                if (left) directions.Add(Vector2.left);
                if (right) directions.Add(Vector2.right);
                _availableDirections = directions.ToArray();
            }

            return _availableDirections;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ghost"))
        {
            // recenter ghosts so they do not get stuck in walls
            other.transform.position = transform.position;
        }
    }
}
