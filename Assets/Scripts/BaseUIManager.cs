using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseUIManager : MonoBehaviour
{
    [SerializeField] private RawImage fadeOutUIImage;
    [SerializeField] private float fadeSpeed = 2.0f;
    
    protected virtual void OnEnable()
    {
        StartCoroutine(Fade(FadeDirection.Out));
    }
    
    protected void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadSceneInternal(sceneName));
    }
    
    private IEnumerator FadeAndLoadSceneInternal(string sceneName)
    {
        yield return Fade(FadeDirection.In);
        SceneManager.LoadScene(sceneName);
    }
    
    private IEnumerator Fade(FadeDirection fadeDirection) 
    {
        float alpha = fadeDirection == FadeDirection.Out ? 1 : 0;
        float targetAlpha = fadeDirection == FadeDirection.Out ? 0 : 1;
        if (fadeDirection == FadeDirection.Out) {
            while (alpha >= targetAlpha)
            {
                SetColorImage (ref alpha, fadeDirection);
                yield return null;
            }
            fadeOutUIImage.enabled = false; 
        } else {
            fadeOutUIImage.enabled = true; 
            while (alpha <= targetAlpha)
            {
                SetColorImage (ref alpha, fadeDirection);
                yield return null;
            }
        }
    }
    
    private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
    {
        fadeOutUIImage.color = new Color (fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
        alpha += Time.deltaTime * fadeSpeed * (fadeDirection == FadeDirection.Out ? -1 : 1);
    }
    
    private enum FadeDirection
    {
        Out,
        In
    }
}
