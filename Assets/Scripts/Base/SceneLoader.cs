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

    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadGamePlay() => StartCoroutine(LoadSceneAsync(GAMEPLAY));

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
}
