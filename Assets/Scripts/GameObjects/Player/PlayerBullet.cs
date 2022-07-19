using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Actor
{
    private Vector2 _startPosition;
    private float _distance = 0;
    private float _totalDistance = 0;
    private float _targetDistance;

    protected override void Awake()
    {
        base.Awake();
        _targetDistance = _camHalfWidth * 2;
    }

    protected override void Update()
    {
        base.Update();
        BulletMove();
    }

    private void BulletMove()
    {
        _transform.Translate(GameConfig.BulletSpeed * Time.deltaTime * Vector3.up);
        _totalDistance = Vector2.Distance(_startPosition, _transform.position) + _distance;

        if (_totalDistance >= _targetDistance)
            ReturnToPool();
    }

    public void Init(Transform gunTransform)
    {
        _transform.up = gunTransform.up;
        _transform.position = gunTransform.position;
        _startPosition = _transform.position;
        gameObject.SetActive(true);
    }

    protected override void CheckIntersectionWithBorders()
    {
        base.CheckIntersectionWithBorders();
        _distance = _totalDistance;
        _startPosition = _transform.position;
    }

    protected override void ReturnToPool()
    {
        _distance = 0;
        _totalDistance = 0;
        base.ReturnToPool();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Asteroid asteroid))
        {
            ReturnToPool();
            asteroid.KillEnemy();
        }

        if (collision.gameObject.TryGetComponent(out Ufo ufo))
        {
            ReturnToPool();
            ufo.KillEnemy();
        }
    }
}
