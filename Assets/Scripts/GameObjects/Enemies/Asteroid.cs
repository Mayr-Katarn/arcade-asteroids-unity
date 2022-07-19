using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Actor
{
    [SerializeField] private Vector2 _sizeBig;
    [SerializeField] private Vector2 _sizeMiddle;
    [SerializeField] private Vector2 _sizeSmall;

    private AsteroidType _asteroidType = AsteroidType.AsteroidBig;
    private float _moveSpeed;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        AsteroidMove();
    }

    public void Init(Vector2 position, Vector3 angle, float moveSpeed, AsteroidType asteroidType = AsteroidType.AsteroidBig)
    {
        _transform.position = position;
        _transform.eulerAngles = angle;
        _moveSpeed = moveSpeed;
        SetType(asteroidType);
        gameObject.SetActive(true);
    }

    public void SetType(AsteroidType type)
    {
        switch (type)
        {
            case AsteroidType.AsteroidSmall:
                _transform.localScale = _sizeSmall;
                break;

            case AsteroidType.AsteroidMiddle:
                _transform.localScale = _sizeMiddle;
                break;

            case AsteroidType.AsteroidBig:
                _transform.localScale = _sizeBig;
                break;
        }

        _asteroidType = type;
    }

    private void AsteroidMove()
    {
        _transform.Translate(_moveSpeed * Time.deltaTime * Vector3.up);
    }

    public void KillEnemy(bool isByPlayer = true)
    {
        EventManager.SendAsteroidDestruction(_asteroidType, _transform, isByPlayer);
        ReturnToPool();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            KillEnemy(false);
            player.KillPlayer();
        }

        if (collision.gameObject.TryGetComponent(out Ufo ufo))
        {
            KillEnemy(false);
            ufo.KillEnemy(false);
        }
    }
}
