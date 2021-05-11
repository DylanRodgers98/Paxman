using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] ghosts;
    private IDictionary<string, MovementReplay> _actors;
    private Tuple<string, Queue<HistoricalDirection>>[] _deserializedReplayFile;

    [SerializeField] public string ReplayFilePath;

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (ghosts == null || ghosts.Length == 0)
        {
            ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        }

        _actors = new Dictionary<string, MovementReplay>(ghosts.Length + 1)
        {
            [player.name] = player.GetComponent<MovementReplay>()
        };

        foreach (GameObject ghost in ghosts)
        {
            _actors[ghost.name] = ghost.GetComponent<MovementReplay>();
        }

        using (FileStream fs = File.OpenRead(ReplayFilePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            _deserializedReplayFile = (Tuple<string, Queue<HistoricalDirection>>[]) binaryFormatter.Deserialize(fs);
        }

        foreach ((string actor, Queue<HistoricalDirection> directions) in _deserializedReplayFile)
        {
            _actors[actor].Directions = directions;
        }
    }
}
