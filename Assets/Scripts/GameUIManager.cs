using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    private const string ScoreTextPrefix = "Score: ";
    private const string LivesTextPrefix = "Lives: ";
    private const string DeathTextPrefix = "YOU DIED\nFinal Score: ";
    
    [SerializeField] protected string mainMenuSceneName;
    [SerializeField] protected GameObject deathElementsParent;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text deathText;
    protected int Score;

    protected virtual void Start()
    {
        deathElementsParent.SetActive(false);
    }

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
        Score = score;
        deathElementsParent.SetActive(true);
        deathText.text = DeathTextPrefix + Score;
    }
}
