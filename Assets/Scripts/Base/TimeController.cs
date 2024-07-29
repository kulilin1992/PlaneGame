using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;

    float defaulyFixedDeltaTime;
    float t;

    //bool isGamePaused;

    float timeScaleBeforePause;

    protected override void Awake()
    {
        base.Awake();
        defaulyFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;
        //Time.fixedDeltaTime = defaulyFixedDeltaTime * Time.timeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }

    public void BulletTime(float inDuration, float outDuration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }

    public void BulletTime(float inDuration, float keepDuration, float outDuration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowInAndKeepAndOutCoroutine(inDuration, keepDuration, outDuration));
    }

    IEnumerator ReturnToNormalTime(float duration)
    {
        t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            //t += Time.deltaTime / duration;
            Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
            Time.fixedDeltaTime = defaulyFixedDeltaTime * Time.timeScale;
            yield return null;
        }
    }
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0f;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused) {
                t += Time.unscaledDeltaTime / duration;
                //t += Time.deltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaulyFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }

    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0f;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused) {
                t += Time.unscaledDeltaTime / duration;
                //t += Time.deltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaulyFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        StartCoroutine(SlowOutCoroutine(outDuration));
    }


    IEnumerator SlowInAndKeepAndOutCoroutine(float inDuration, float keepDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    public void PauseGame()
    {
        timeScaleBeforePause = Time.timeScale;
        //Debug.Log("PauseGame:" + timeScaleBeforePause);
        Time.timeScale = 0f;
        //isGamePaused = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = timeScaleBeforePause;
        //isGamePaused = false;
    }
}
