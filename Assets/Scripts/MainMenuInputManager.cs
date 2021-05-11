using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuInputManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName;

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}