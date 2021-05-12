using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class MainLevelUIManager : BaseLevelUIManager
{
    [SerializeField] private string highScoresFileName = "HighScores.dat";
    [SerializeField] private InputField nameInputField;
    private string _highScoresFilePath;
    private IList<Tuple<string, int>> _highScores;

    protected override void Start()
    {
        base.Start();
        _highScoresFilePath = $"{Application.persistentDataPath}/{highScoresFileName}";
    }

    public void OnSubmitButtonClick()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        
        if (File.Exists(_highScoresFilePath))
        {
            using (FileStream fs = File.OpenRead(_highScoresFilePath))
            {
                _highScores = (List<Tuple<string, int>>) binaryFormatter.Deserialize(fs);
            }
        }
        else
        {
            _highScores = new List<Tuple<string, int>>();
        }

        _highScores.Add(Tuple.Create(nameInputField.text, Score));
        
        using (FileStream fs = File.OpenWrite(_highScoresFilePath))
        {
            binaryFormatter.Serialize(fs, _highScores);
        }

        FadeAndLoadScene(mainMenuSceneName);
    }
}