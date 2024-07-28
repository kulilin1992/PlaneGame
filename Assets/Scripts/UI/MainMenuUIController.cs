using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] Button startButton;
    bool isTrigger;

    void Awake()
    {
        isTrigger = false;
    }

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
        GameManager.GameState = GameState.Playing;
    }

    void OnStartButtonClick()
    {
        if (!isTrigger)
        {
            SceneLoader.Instance.LoadGamePlay();
            isTrigger = true;
        }
    }
}
