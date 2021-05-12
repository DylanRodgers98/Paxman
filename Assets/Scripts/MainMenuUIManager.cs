using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string viewHighScoresSceneName;
    [SerializeField] private string viewReplaySceneName;
    [SerializeField] private Button viewReplayButton;

    private void Start()
    {
        if (!File.Exists($"{Application.persistentDataPath}/{ReplayManager.ReplayFileName}"))
        {
            viewReplayButton.interactable = false;
        }
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnViewReplayButtonClick()
    {
        SceneManager.LoadScene(viewReplaySceneName);
    }

    public void OnViewLeaderboardButtonClick()
    {
        SceneManager.LoadScene(viewHighScoresSceneName);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}