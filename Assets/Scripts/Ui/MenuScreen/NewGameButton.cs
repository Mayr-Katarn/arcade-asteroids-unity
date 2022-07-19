using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    public void OnNewGame()
    {
        EventManager.SendToggleMenu();
        EventManager.SendNewGame();
    }
}
