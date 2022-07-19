using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    #region FIELDS
    public static UnityEvent OnNewGame = new();
    public static UnityEvent OnGameOver = new();
    public static UnityEvent<float> OnUpdateScore = new();
    public static UnityEvent OnToggleMenu = new();
    public static UnityEvent<AsteroidType, Transform, bool> OnAsteroidDestruction = new();
    public static UnityEvent<bool> OnUfoDestruction = new();
    public static UnityEvent<int> OnPlayerKill = new();
    public static UnityEvent<SoundType> OnPlaySound = new();
    public static UnityEvent OnStopPlayThruster = new();
    #endregion

    #region METHODS
    public static void SendNewGame() => OnNewGame.Invoke();
    public static void SendGameOver() => OnGameOver.Invoke();
    public static void SendUpdateScore(float score) => OnUpdateScore.Invoke(score);
    public static void SendToggleMenu() => OnToggleMenu.Invoke();
    public static void SendAsteroidDestruction(AsteroidType asteroidType, Transform oldTransform, bool isByPlayer) => OnAsteroidDestruction.Invoke(asteroidType, oldTransform, isByPlayer);
    public static void SendUfoDestruction(bool isByPlayer) => OnUfoDestruction.Invoke(isByPlayer);
    public static void SendPlayerKill(int playerLives) => OnPlayerKill.Invoke(playerLives);
    public static void SendPlaySound(SoundType soundType) => OnPlaySound.Invoke(soundType);
    public static void SendStopPlayThruster() => OnStopPlayThruster.Invoke();
    #endregion
}
