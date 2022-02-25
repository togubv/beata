using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Generator generator;
    public Player player;
    [SerializeField] private GameUI game_ui;

    public GameState gameState;
    public int mistakes, multiplier, score;
    public GameObject goPlayer;
    private float speed = 10, savedspeed;
    public float Speed => speed;

    int currentlvl = 1;

    private void Start()
    {
        savedspeed = speed;
        GameStateMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.Game)
            {
                GameStatePause();
                game_ui.TogglePausePanel(true);
            }
        }
    }

    public void GameStateGame()
    {
        gameState = GameState.Game;
        Time.timeScale = 1;
        speed = savedspeed;

        //hide all menu
    }

    public void GameStateMenu()
    {
        gameState = GameState.Menu;
        savedspeed = speed;
        speed = 0;
    }

    public void GameStatePause()
    {
        gameState = GameState.Pause;
        Time.timeScale = 0;
    }

    public void ZeroingMultiplier(int position)
    {
        multiplier = 0;
        mistakes++;

        game_ui.RefreshUI(multiplier, score, false, false);
        generator.DeleteQuads(position);
    }

    public void AddScore(int position)
    {
        multiplier++;
        score += 10 * multiplier;

        game_ui.RefreshUI(multiplier, score, true, false);
        generator.DeleteQuads(position);
    }

    public void FinishLevel()
    {
        game_ui.FinushLevel(score);
        GameStateMenu();
    }

    public void RestartGame()
    {
        GameStateGame();

        mistakes = 0;
        multiplier = 0;

        game_ui.RefreshUI(multiplier, score, false, false);
        game_ui.StartLevel();

        generator.DeletePrefabs();
        currentlvl++;
        speed += 4;
        generator.GenerateLevel(20, 10, currentlvl);
    }
}

public enum GameState
{
    Pause,
    Menu,
    Game
}
