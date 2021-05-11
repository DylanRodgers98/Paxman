using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class HighScoresLoadBehaviour : MonoBehaviour
{
    private const string NoScoresText = "No high scores available";
    [SerializeField] private string highScoresFileName = "HighScores.dat";
    [SerializeField] private int numberOfHighScoresToShow = 10;
    [SerializeField] private Text highScoresText;
    private string _highScoresFilePath;
    private List<Tuple<string, int>> _deserializedHighScoresFile;
    private Tuple<string, int>[] _highScores;
    
    private void Start()
    {
        _highScoresFilePath = $"{Application.persistentDataPath}/{highScoresFileName}";
        
        if (!File.Exists(_highScoresFilePath))
        {
            highScoresText.text = NoScoresText;
            return;
        }
        
        using (FileStream fs = File.OpenRead(_highScoresFilePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            _deserializedHighScoresFile = (List<Tuple<string, int>>) binaryFormatter.Deserialize(fs);
        }
        
        _deserializedHighScoresFile.Sort((x, y) => y.Item2.CompareTo(x.Item2));
        _highScores = _deserializedHighScoresFile.Take(numberOfHighScoresToShow).ToArray();

        if (_highScores.Length == 0)
        {
            highScoresText.text = NoScoresText;
            return;
        }

        highScoresText.text = "";
        for (int i = 0; i < _highScores.Length; i++)
        {
            (string playerName, int score) = _highScores[i];
            highScoresText.text += $"{i + 1}. {playerName} - {score}\n";
        }
    }
}
