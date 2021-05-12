using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ReplaySerializer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] ghosts;
    private MovementBehaviour _playerMovementBehaviour;
    private Tuple<GameObject, MovementBehaviour>[] _ghostMovementBehaviours;
    private GameObject _currentGhost;
    private MovementBehaviour _currentGhostMovementBehaviour;
    private Tuple<string, Queue<HistoricalDirection>>[] _directionHistory;
    
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

        _directionHistory = new Tuple<string, Queue<HistoricalDirection>>[ghosts.Length + 1];
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
        Queue<HistoricalDirection> playerDirectionHistory = new Queue<HistoricalDirection>();
        foreach ((float time, Vector2 direction) in _playerMovementBehaviour.DirectionHistory)
        {
            playerDirectionHistory.Enqueue(new HistoricalDirection(time, direction.x, direction.y));
        }

        _directionHistory[0] = Tuple.Create(player.name, playerDirectionHistory);

        for (int i = 0; i < _ghostMovementBehaviours.Length; i++)
        {
            Queue<HistoricalDirection> ghostDirectionHistory = new Queue<HistoricalDirection>();
            (GameObject ghost, MovementBehaviour ghostMovementBehaviour) = _ghostMovementBehaviours[i];
            foreach ((float time, Vector2 direction) in ghostMovementBehaviour.DirectionHistory)
            {
                ghostDirectionHistory.Enqueue(new HistoricalDirection(time, direction.x, direction.y));
            }
            _directionHistory[i + 1] = Tuple.Create(ghost.name, ghostDirectionHistory);
        }

        using (FileStream fs = File.OpenWrite($"{Application.persistentDataPath}/{ReplayManager.ReplayFileName}"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, _directionHistory);
        }
    }
}
