using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainLevelUIManager : GameUIManager
{
    private const string DeathTextPrefix = "YOU DIED\nFinal Score: ";

    [SerializeField] private string highScoresFileName = "HighScores.dat";
    [SerializeField] private GameObject deathElementsParent;
    [SerializeField] private Text deathText;
    [SerializeField] private InputField nameInputField;
    private int _score;
    private string _highScoresFilePath;
    private IList<Tuple<string, int>> _highScores;

    protected override void OnEnable()
    {
        PlayerDataHolder.OnFinalScore += ShowDeathText;
    }

    protected override void OnDisable()
    {
        PlayerDataHolder.OnFinalScore -= ShowDeathText;
    }

    private void Start()
    {
        deathElementsParent.SetActive(false);
        _highScoresFilePath = $"{Application.persistentDataPath}/{highScoresFileName}";
    }

    private void ShowDeathText(int score)
    {
        _score = score;
        deathElementsParent.SetActive(true);
        deathText.text = DeathTextPrefix + _score;
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

        _highScores.Add(Tuple.Create(nameInputField.text, _score));
        
        using (FileStream fs = File.OpenWrite(_highScoresFilePath))
        {
            binaryFormatter.Serialize(fs, _highScores);
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}