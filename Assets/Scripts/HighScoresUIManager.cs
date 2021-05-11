using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoresUIManager : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName;

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
