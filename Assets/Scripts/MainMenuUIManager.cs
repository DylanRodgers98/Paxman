using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : BaseUIManager
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
        FadeAndLoadScene(gameSceneName);
    }

    public void OnViewReplayButtonClick()
    {
        FadeAndLoadScene(viewReplaySceneName);
    }

    public void OnViewLeaderboardButtonClick()
    {
        FadeAndLoadScene(viewHighScoresSceneName);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}