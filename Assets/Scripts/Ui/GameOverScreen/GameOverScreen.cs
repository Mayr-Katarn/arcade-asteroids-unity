using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private void OnEnable()
    {
        EventManager.OnNewGame.AddListener(NewGame);
        EventManager.OnGameOver.AddListener(GameOver);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void NewGame()
    {
        gameObject.SetActive(false);
    }

    private void GameOver()
    {
        gameObject.SetActive(true);
        string text = $"SCORE: {GameConfig.Score}";
        _scoreText.text = text;
    }
}
