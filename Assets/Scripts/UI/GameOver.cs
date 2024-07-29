using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Canvas hudCanvas;

    [SerializeField] AudioData confirmGameOverSound;

    int exitStateID = Animator.StringToHash("GameOverScreenExit");

    Canvas canvas;

    Animator animator;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();

        canvas.enabled = false;
        animator.enabled = false;
    }

    void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;

        playerInput.onGameOver += OnConfirmGameOver;
    }

    void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        playerInput.onGameOver -= OnConfirmGameOver;
    }

    void OnConfirmGameOver()
    {
        AudioManager.Instance.PlaySFX(confirmGameOverSound);
        playerInput.DisableAllInputs();
        animator.Play(exitStateID);
        //TODO 加载积分场景
        //SceneLoader.Instance.LoadMainMenu();
        SceneLoader.Instance.LoadScoreScene();
    }

    void OnGameOver()
    {
        hudCanvas.enabled = false;
        canvas.enabled = true;
        animator.enabled = true;
        playerInput.DisableAllInputs();
    }


    // Ainimator event
    void EnableGameOverScreenInput()
    {
        playerInput.EnableGameOverScreenInput();
    }
}
