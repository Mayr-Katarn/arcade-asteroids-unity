using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResumeGameButton : MonoBehaviour
{
    private Button _resumeGameButton;

    private void Awake()
    {
        _resumeGameButton = GetComponent<Button>();
    }

    private void Update()
    {
        _resumeGameButton.interactable = !GameConfig.IsGameOver;
    }

    public void OnResumeGame()
    {
        EventManager.SendToggleMenu();
    }
}
