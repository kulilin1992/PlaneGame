using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistenSingleton<SceneLoader>
{
    [SerializeField] Image transitionImage;
    [SerializeField] float fadeTime = 3.5f;

    Color color;
    const string GAMEPLAY = "GamePlay";

    const string MAIN_MENU = "Menu";

    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        var loading = SceneManager.LoadSceneAsync(sceneName);
        loading.allowSceneActivation = false;


        transitionImage.gameObject.SetActive(true);

        //Fade out
        while (color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;
            yield return null;
        }

        //Load(sceneName);
        yield return new WaitUntil(() => loading.progress >= 0.9f);

        loading.allowSceneActivation = true;
        //Fade in
        while (color.a > 0f)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;
            yield return null;
        }
        transitionImage.gameObject.SetActive(false);
    }

    // public void LoadGamePlay() => StartCoroutine(LoadSceneAsync(GAMEPLAY));
    // public void LoadMainMenu() => StartCoroutine(LoadSceneAsync(MAIN_MENU));
    public void LoadGamePlay()
    {
        StopAllCoroutines();
        StartCoroutine(LoadSceneAsync(GAMEPLAY));
    }
    public void LoadMainMenu()
    {
        StopAllCoroutines();
        StartCoroutine(LoadSceneAsync(MAIN_MENU));
    }

}
