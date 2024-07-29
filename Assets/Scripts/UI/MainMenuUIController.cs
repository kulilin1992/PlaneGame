using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{

    [Header("====== CANVAS ======")]
    [SerializeField] Canvas mainMenuCanvas;

    [Header("====== BUTTONS ======")]
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonOptions;
    [SerializeField] Button buttonExit;
    bool isTrigger;

    void Awake()
    {
        isTrigger = false;
        mainMenuCanvas.enabled = true;
    }

    void OnEnable()
    {
        //buttonStart.Select();
        // buttonStart.onClick.AddListener(OnStartButtonClick);
        // buttonOptions.onClick.AddListener(OnButtonOptionsClick);
        // buttonExit.onClick.AddListener(OnButtonExitClick);
        PressedButtonBehaviour.buttonFunctionTable.Add(buttonStart.gameObject.name, OnStartButtonClick);
        PressedButtonBehaviour.buttonFunctionTable.Add(buttonOptions.gameObject.name, OnButtonOptionsClick);
        PressedButtonBehaviour.buttonFunctionTable.Add(buttonExit.gameObject.name, OnButtonExitClick);
    }

    void OnDisable()
    {
        // buttonStart.onClick.RemoveListener(OnStartButtonClick);
        // buttonOptions.onClick.RemoveListener(OnButtonOptionsClick);
        // buttonExit.onClick.RemoveListener(OnButtonExitClick);
        buttonStart.onClick.RemoveListener(OnStartButtonClick);
        PressedButtonBehaviour.buttonFunctionTable.Clear();
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }

    void OnStartButtonClick()
    {
        if (!isTrigger)
        {
            mainMenuCanvas.enabled = false;
            SceneLoader.Instance.LoadGamePlay();
            isTrigger = true;
        }
    }

    void OnButtonOptionsClick()
    {
        //UIInput.Instance.SelectUI(buttonOptions);
    }

    void OnButtonExitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
