using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuInputManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string viewLeaderboardSceneName;
    [SerializeField] private string viewReplaysSceneName;

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnViewLeaderboardButtonClick()
    {
        SceneManager.LoadScene(viewLeaderboardSceneName);
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