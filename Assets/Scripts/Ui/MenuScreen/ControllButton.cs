using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ControllButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _toggleControlSchemeButtonText;

    private const string KEYBOARD_INPUT = "CONTROL SCHEME:\nkeyboard";
    private const string KEYBOARD_MOUSE_INPUT = "CONTROL SCHEME:\nkeyboard + mouse";

    public void OnToggleControlScheme()
    {
        switch (GameConfig.InputType)
        {
            case InputType.Keyboard:
                GameConfig.InputType = InputType.MouseKeyboard;
                _toggleControlSchemeButtonText.text = KEYBOARD_MOUSE_INPUT;
                break;

            case InputType.MouseKeyboard:
                GameConfig.InputType = InputType.Keyboard;
                _toggleControlSchemeButtonText.text = KEYBOARD_INPUT;
                break;
        }
    }
}
