using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] Button startButton;

    void OnEnable()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
    }

    void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStartButtonClick);
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    void OnStartButtonClick()
    {
        SceneLoader.Instance.LoadGamePlay();
    }
}
