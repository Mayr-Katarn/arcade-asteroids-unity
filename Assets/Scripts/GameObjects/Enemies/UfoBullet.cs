using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoBullet : Actor
{
    private Vector2 _direction;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        CheckIntersectionWithBorders();
        BulletMove();
    }

    private void BulletMove()
    {
        _transform.Translate(GameConfig.BulletSpeed * Time.deltaTime * Vector3.up);

    }

    public void Init(Vector2 ufoPosition, Vector2 playerPosition)
    {
        _transform.position = ufoPosition;
        _direction = playerPosition - ufoPosition;
        _transform.up = _direction.normalized;
        gameObject.SetActive(true);
    }

    protected override void CheckIntersectionWithBorders()
    {
        float x = _transform.position.x;
        float y = _transform.position.y;
        bool isReachBorders = x > _camHalfWidth || x < -_camHalfWidth || y > _camHalfHeight || y < -_camHalfHeight;

        if (isReachBorders)
            ReturnToPool();
    }

    protected override void ReturnToPool()
    {

        base.ReturnToPool();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            ReturnToPool();
            player.KillPlayer();
        }
    }
}
