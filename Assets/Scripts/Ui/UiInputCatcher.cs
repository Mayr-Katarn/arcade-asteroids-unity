using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInputCatcher : MonoBehaviour
{
    [SerializeField] private GameObject _menu;

    private void OnEnable()
    {
        EventManager.OnToggleMenu.AddListener(ToggleMenu);
        EventManager.OnGameOver.AddListener(GameOver);
    }

    private void Update()
    {
        bool isButtonDown = Input.GetButtonDown("Menu");

        if (isButtonDown && !GameConfig.IsGameOver)
            ToggleMenu();
    }

    private void ToggleMenu()
    {
        _menu.SetActive(!_menu.activeInHierarchy);

        if (_menu.activeInHierarchy)
            GameConfig.PauseGame();
        else
            GameConfig.ResumeGame();
    }

    private void GameOver()
    {
        _menu.SetActive(true);
    }
}
