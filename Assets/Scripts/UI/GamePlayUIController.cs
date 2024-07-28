using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIController : MonoBehaviour
{
    [Header("=========PLAYER INPUT===========")]
    [SerializeField] PlayerInput playerInput;

    [Header("=========Canvas===========")]
    [SerializeField] Canvas hudCanvas;
    [SerializeField] Canvas menuCanvas;

     [Header("=========Canvas===========")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button mainMenuButton;

    [Header("=========SFX===========")]
    [SerializeField] AudioData pauseSFX;
    [SerializeField] AudioData unPauseSFX;

    void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnPause += UnPause;

        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        optionButton.onClick.AddListener(OnOptionButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);

        // PressedButtonBehaviour.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClicked);
        // PressedButtonBehaviour.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClicked);
        // PressedButtonBehaviour.buttonFunctionTable.Add(optionButton.gameObject.name, OnOptionButtonClicked);
        
    }

    void Disable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnPause -= UnPause;

        resumeButton.onClick.RemoveAllListeners();
        optionButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
    }

    void UnPause()
    {
        // resumeButton.Select();
        // resumeButton.animator.SetTrigger("Pressed");
        OnResumeButtonClicked();
        AudioManager.Instance.PlaySFX(unPauseSFX);
    }


    void Pause()
    {
        //Time.timeScale = 0f;
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.PauseGame();
        if (hudCanvas != null)
            hudCanvas.enabled = false;
        if (menuCanvas != null)
            menuCanvas.enabled = true;
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();
       // UIInput.Instance.SelectUI(resumeButton);
       AudioManager.Instance.PlaySFX(pauseSFX);
    }

    void OnResumeButtonClicked()
    {
        //Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.ResumeGame();
        if (hudCanvas != null)
           hudCanvas.enabled = true;
        if (menuCanvas != null)
           menuCanvas.enabled = false; // TODO: check if this is needed
        playerInput.EnablePlayerInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    void OnOptionButtonClicked()
    {

    }
    void OnMainMenuButtonClicked()
    {
        menuCanvas.enabled = false;
        //playerInput.EnablePlayerInput();
        //playerInput.SwitchToFixedUpdateMode();
        SceneLoader.Instance.LoadMainMenu();
    }
}
