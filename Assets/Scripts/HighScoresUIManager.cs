using UnityEngine;

public class HighScoresUIManager : BaseUIManager
{
    [SerializeField] private string mainMenuSceneName;

    public void OnBackButtonClick()
    {
        FadeAndLoadScene(mainMenuSceneName);
    }
}
