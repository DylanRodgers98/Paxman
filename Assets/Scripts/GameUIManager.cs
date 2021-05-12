using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    private const string ScoreTextPrefix = "Score: ";
    private const string LivesTextPrefix = "Lives: ";
    
    [SerializeField] protected string mainMenuSceneName;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    
    public void ReturnToMainMenuOnClick()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    protected virtual void OnEnable()
    {
        PlayerDataHolder.OnScoreChanged += UpdateScoreText;
        PlayerDataHolder.OnLivesChanged += UpdateLivesText;
    }

    protected virtual void OnDisable()
    {
        PlayerDataHolder.OnScoreChanged -= UpdateScoreText;
        PlayerDataHolder.OnLivesChanged -= UpdateLivesText;
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
