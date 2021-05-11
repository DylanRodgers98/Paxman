using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewReplaysInputManager : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName;

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
