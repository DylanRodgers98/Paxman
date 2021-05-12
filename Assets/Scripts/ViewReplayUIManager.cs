using UnityEngine.SceneManagement;

public class ViewReplayUIManager : GameUIManager
{
    public void OnMainMenuReturnButtonClick()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
