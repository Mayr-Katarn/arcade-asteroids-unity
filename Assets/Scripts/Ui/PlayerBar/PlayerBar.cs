using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Transform _playerLivesPanel;
    [SerializeField] private GameObject _playerLiveIconPrefab;

    private const float ICON_SIZE = 20f;
    private readonly List<GameObject> _playerLivesIcons = new();

    private void OnEnable()
    {
        EventManager.OnNewGame.AddListener(NewGame);
        EventManager.OnGameOver.AddListener(GameOver);
        EventManager.OnPlayerKill.AddListener(PlayerKill);
        EventManager.OnUpdateScore.AddListener(UpdateScore);
    }

    private void Start()
    {
        for (int i = 0; i < GameConfig.StartPlayerLives; i++)
        {
            GameObject icon = Instantiate(_playerLiveIconPrefab, _playerLivesPanel);
            icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(ICON_SIZE * i, 0);
            icon.SetActive(false);
            _playerLivesIcons.Add(icon);
        }

        gameObject.SetActive(false);
    }

    private void UpdateScore(float score)
    {
        _scoreText.text = score.ToString();
    }

    private void NewGame()
    {
        foreach (GameObject icon in _playerLivesIcons)
        {
            icon.SetActive(true);
        }

        gameObject.SetActive(true);
    }

    private void GameOver()
    {
        gameObject.SetActive(false);
    }

    private void PlayerKill(int playerLives)
    {
        _playerLivesIcons[playerLives].SetActive(false);
    }
}
