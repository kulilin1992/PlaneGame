using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIController : MonoBehaviour
{
    [Header("====== BACKGROUND ======")]
    [SerializeField] Image background;

    [SerializeField ] Sprite[] backgroundImages;

    [Header("====== SCORE ======")]
    [SerializeField] Canvas scoreScreenCanvas;
    [SerializeField] Text playerScoreText;
    [SerializeField] Button buttonMainMenu;

    [SerializeField] Transform highScoreBoard;

    [Header("====== NEW HIGH SCORE ======")]
    [SerializeField] Canvas newHighScoreScreenCanvas;
    [SerializeField] Button btn_Cancel;
    [SerializeField] Button btn_Submit;

    [SerializeField] InputField playerNameInputField;
    

    void Start()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ShowRandomBackground();
        if (ScoreManager.Instance.HasNewHighScore)
        {
            ShowNewHighScoreScreen();
        }
        else 
        {
            ShowScoringScreen();
        }

        PressedButtonBehaviour.buttonFunctionTable.Add(buttonMainMenu.gameObject.name, OnButtonMainMenuClicked);
        PressedButtonBehaviour.buttonFunctionTable.Add(btn_Submit.gameObject.name, OnButtonSubmitClicked);
        PressedButtonBehaviour.buttonFunctionTable.Add(btn_Cancel.gameObject.name, HideNewHighScoreScreen);
    }
    void OnEnable()
    {
        // PressedButtonBehaviour.buttonFunctionTable.Add(buttonMainMenu.gameObject.name, OnButtonMainMenuClicked);
        GameManager.GameState = GameState.Score;
    }

    void OnDisable()
    {
        PressedButtonBehaviour.buttonFunctionTable.Clear();
    }
    void ShowRandomBackground()
    {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
    }

    void ShowScoringScreen()
    {
        scoreScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);
        //TODO 更新高分排行榜

        UpdateHighScoreBoard();
    }

    void ShowNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = true;
    }

    void OnButtonMainMenuClicked()
    {
        scoreScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenu();
    }

    void OnButtonSubmitClicked()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
            ScoreManager.Instance.SavePlayerScoreData();
        }
        HideNewHighScoreScreen();
    }

    void UpdateHighScoreBoard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().playerScoresList;

        for (int i = 0; i < highScoreBoard.childCount; i++) {
            var child = highScoreBoard.GetChild(i);
            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].name;
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
        }
    }

    void HideNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = false;

        //ScoreManager.Instance.SavePlayerScoreData();
        ShowRandomBackground();
        ShowScoringScreen();
    }
}
