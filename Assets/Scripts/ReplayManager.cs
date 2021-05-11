using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] ghosts;
    private IDictionary<string, MovementBehaviour> _movementBehaviours;
    private Queue<HistoricalDirection> _historicalDirections;
    private HistoricalDirection _currentHistoricalDirection;
    private Vector2 _currentDirection;

    public string ReplayFilePath { private get; set; }
    
    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        if (ghosts == null || ghosts.Length == 0)
        {
            ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        }

        _movementBehaviours = new Dictionary<string, MovementBehaviour>(ghosts.Length + 1)
        {
            [player.name] = player.GetComponent<MovementBehaviour>()
        };
        
        foreach (GameObject ghost in ghosts)
        {
            _movementBehaviours[ghost.name] = ghost.GetComponent<MovementBehaviour>();
        }

        using (FileStream fs = File.OpenRead(ReplayFilePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            _historicalDirections = (Queue<HistoricalDirection>) binaryFormatter.Deserialize(fs);
        }
        
        GetNextHistoricalDirection();
    }

    private void Update()
    {
        while (_currentHistoricalDirection?.Time >= Time.timeSinceLevelLoad)
        {
            _currentDirection = new Vector2(_currentHistoricalDirection.X, _currentHistoricalDirection.Y);
            _movementBehaviours[_currentHistoricalDirection.Actor].SetDirection(_currentDirection);
            GetNextHistoricalDirection();
        }
    }

    private void GetNextHistoricalDirection()
    {
        _currentHistoricalDirection = _historicalDirections.Count > 0 ? _historicalDirections.Dequeue() : null;
    }
}
