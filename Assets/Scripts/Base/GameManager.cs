using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistenSingleton<GameManager>
{
    public static System.Action onGameOver;
    public static GameState GameState { get => Instance.gameState; set => Instance.gameState = value; }
    [SerializeField] GameState gameState = GameState.Playing;
}
public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Score
}
