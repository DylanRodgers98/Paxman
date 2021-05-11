using UnityEngine;
using UnityEngine.UI;

public class MainLevelUIManager : MonoBehaviour
{
    private const string ScoreTextPrefix = "Score: ";
    private const string LivesTextPrefix = "Lives: ";
    
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    private void OnEnable()
    {
        PlayerController.OnScoreChanged += UpdateScoreText;
        PlayerController.OnLivesChanged += UpdateLivesText;
    }

    private void OnDisable()
    {
        PlayerController.OnScoreChanged -= UpdateScoreText;
        PlayerController.OnLivesChanged -= UpdateLivesText;
    }

    private void UpdateScoreText(int score)
    {
        scoreText.text = ScoreTextPrefix + score;
    }

    private void UpdateLivesText(int lives)
    {
        livesText.text = LivesTextPrefix + lives;
    }
}
