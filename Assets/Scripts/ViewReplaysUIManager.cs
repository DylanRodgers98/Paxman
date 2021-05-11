using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewReplaysUIManager : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName;

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
