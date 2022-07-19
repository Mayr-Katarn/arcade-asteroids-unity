using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _thrusterSound;
    [SerializeField] private AudioSource _fireSound;
    [SerializeField] private AudioSource _explosionSound;

    private bool _isTrusterWorks = false;

    private void OnEnable()
    {
        EventManager.OnPlaySound.AddListener(PlaySound);
        EventManager.OnStopPlayThruster.AddListener(StopPlayThruster);
        EventManager.OnGameOver.AddListener(GameOver);
    }

    private void PlaySound(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Thruster:
                _isTrusterWorks = true;
                break;

            case SoundType.Fire:
                _fireSound.Play();
                break;

            case SoundType.Explosion:
                _explosionSound.Play();
                break;
        }
    }

    private void Update()
    {
        ThrusterSoundHandler();
    }

    private void ThrusterSoundHandler()
    {
        if (_isTrusterWorks && !_thrusterSound.isPlaying)
            _thrusterSound.Play();
        else if (!_isTrusterWorks && _thrusterSound.isPlaying)
            _thrusterSound.Stop();
    }

    private void StopPlayThruster()
    {
        _isTrusterWorks = false;
    }

    private void GameOver()
    {
        StopPlayThruster();
    }
}
