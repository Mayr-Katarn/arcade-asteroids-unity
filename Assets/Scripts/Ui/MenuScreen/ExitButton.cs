using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button _exitGameButton;

    private void Awake()
    {
        _exitGameButton = GetComponent<Button>();
    }

    private void Start()
    {
        _exitGameButton.interactable = Application.platform != RuntimePlatform.WebGLPlayer;
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
