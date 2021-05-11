using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ReplaySerializer : MonoBehaviour
{
    [SerializeField] private string replayFileDirectoryName = "replays";
    [SerializeField] private string replayFileExtension = ".dat";
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] ghosts;
    private MovementBehaviour _playerMovementBehaviour;
    private IList<HistoricalDirection> _playerDirectionHistory;
    private Tuple<GameObject, MovementBehaviour>[] _ghostMovementBehaviours;
    private string _replayFileDirectory;
    private GameObject _currentGhost;
    private MovementBehaviour _currentGhostMovementBehaviour;
    private List<HistoricalDirection> _directionHistory;
    private string _filePath;
    
    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        _playerMovementBehaviour = player.GetComponent<MovementBehaviour>();

        if (ghosts == null || ghosts.Length == 0)
        {
            ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        }

        _ghostMovementBehaviours = new Tuple<GameObject, MovementBehaviour>[ghosts.Length];
        
        for (int i = 0; i < ghosts.Length; i++)
        {
            _currentGhost = ghosts[i];
            _currentGhostMovementBehaviour = _currentGhost.GetComponent<MovementBehaviour>();
            _ghostMovementBehaviours[i] = Tuple.Create(_currentGhost, _currentGhostMovementBehaviour);
        }
        
        _replayFileDirectory = $"{Application.persistentDataPath}/{replayFileDirectoryName}";
        Directory.CreateDirectory(_replayFileDirectory);
    }

    private void OnEnable()
    {
        PlayerDataHolder.OnPlayerDied += SerializeReplay;
    }

    private void OnDisable()
    {
        PlayerDataHolder.OnPlayerDied -= SerializeReplay;
    }

    private void SerializeReplay()
    {
        _directionHistory = new List<HistoricalDirection>();
        foreach ((float time, Vector2 direction) in _playerMovementBehaviour.DirectionHistory)
        {
            _directionHistory.Add(new HistoricalDirection(player.name, time, direction.x, direction.y));
        }
        
        foreach ((GameObject ghost, MovementBehaviour ghostMovementBehaviour) in _ghostMovementBehaviours)
        {
            foreach ((float time, Vector2 direction) in ghostMovementBehaviour.DirectionHistory)
            {
                _directionHistory.Add(new HistoricalDirection(ghost.name, time, direction.x, direction.y));
            }
        }
        
        _directionHistory.Sort();

        _filePath = $"{_replayFileDirectory}/{DateTimeOffset.Now.ToUnixTimeMilliseconds()}{replayFileExtension}";
        using (FileStream fs = File.Create(_filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, _directionHistory);
        }
    }

    [Serializable]
    private class HistoricalDirection : IComparable<HistoricalDirection>
    {
        public string Actor { get; set; }
        public float Time { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public HistoricalDirection(string actor, float time, float x, float y)
        {
            Actor = actor;
            Time = time;
            X = x;
            Y = y;
        }

        public int CompareTo(HistoricalDirection other)
        {
            return Time.CompareTo(other.Time);
        }
    }
}
