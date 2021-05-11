using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string viewHighScoresSceneName;
    [SerializeField] private string viewReplaysSceneName;

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnViewLeaderboardButtonClick()
    {
        SceneManager.LoadScene(viewHighScoresSceneName);
    }

    public void OnViewReplaysButtonClick()
    {
        SceneManager.LoadScene(viewReplaysSceneName);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}