using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    [SerializeField] private int _startPlayerLives = 3;
    [SerializeField] private float _bulletSpeed = 4f;
    [SerializeField] private int _asteroidBigPointsCost = 20;
    [SerializeField] private int _asteroidMiddlePointsCost = 50;
    [SerializeField] private int _asteroidSmallPointsCost = 100;
    [SerializeField] private int _ufoPointsCost = 200;

    private static InputType _inputType = InputType.Keyboard;
    private static GameConfig instance;
    public static bool isGamePaused = false;
    private static bool _isGameOver = true;
    private static int _score = 0;

    public static int StartPlayerLives
    {
        get => instance._startPlayerLives;
    }

    public static float BulletSpeed => instance._bulletSpeed;

    public static bool IsGameOver
    {
        get => _isGameOver;
        set => _isGameOver = value;
    }

    public static int Score
    {
        get => _score;
        set => _score = value;
    }

    public static InputType InputType
    {
        get => _inputType;
        set => _inputType = value;
    }

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        EventManager.OnNewGame.AddListener(NewGame);
        EventManager.OnGameOver.AddListener(GameOver);
    }

    public static void SetScore(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.AsteroidSmall:
                Score += instance._asteroidSmallPointsCost;
                break;

            case EnemyType.AsteroidMiddle:
                Score += instance._asteroidMiddlePointsCost;
                break;

            case EnemyType.AsteroidBig:
                Score += instance._asteroidBigPointsCost;
                break;

            case EnemyType.Ufo:
                Score += instance._ufoPointsCost;
                break;
        }

        EventManager.SendUpdateScore(Score);
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
        isGamePaused = true;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    private void NewGame()
    {
        IsGameOver = false;
        Score = 0;
        EventManager.SendUpdateScore(Score);
        ResumeGame();
    }

    private void GameOver()
    {
        IsGameOver = true;
    }
}
