using System;
using UnityEngine;
using UnityEngine.UI;

public class MainLevelUIManager : MonoBehaviour
{
    private const string ScoreTextPrefix = "Score: ";
    private const string LivesTextPrefix = "Lives: ";
    private const string DeathTextPrefix = "YOU DIED\nFinal Score: ";
    
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text deathText;

    private void OnEnable()
    {
        PlayerDataHolder.OnScoreChanged += UpdateScoreText;
        PlayerDataHolder.OnLivesChanged += UpdateLivesText;
        PlayerDataHolder.OnFinalScore += ShowDeathText;
    }

    private void OnDisable()
    {
        PlayerDataHolder.OnScoreChanged -= UpdateScoreText;
        PlayerDataHolder.OnLivesChanged -= UpdateLivesText;
        PlayerDataHolder.OnFinalScore -= ShowDeathText;
    }

    private void Start()
    {
        deathText.enabled = false;
    }

    private void UpdateScoreText(int score)
    {
        scoreText.text = ScoreTextPrefix + score;
    }

    private void UpdateLivesText(int lives)
    {
        livesText.text = LivesTextPrefix + lives;
    }

    private void ShowDeathText(int score)
    {
        deathText.enabled = true;
        deathText.text = DeathTextPrefix + score;
    }
}
