using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardInputManager : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName;

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
