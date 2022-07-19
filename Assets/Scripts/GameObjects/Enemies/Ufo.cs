using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ufo : Actor
{
    [SerializeField] private int _bulletPoolSize = 4;
    [SerializeField] private UfoBullet _bulletPrefab;

    private Transform _playerTransform;
    private float _speed;
    private bool _isRightMoving;
    private const int INACCESSIBLE_AREA_PERSENT = 20;

    private Transform _bulletContainer;
    private Pool<UfoBullet> _bulletPool;
    private float _fireLastTime;
    private const float MIN_FIRE_COOLDOWN = 2f;
    private const float MAX_FIRE_COOLDOWN = 5f;

    protected override void Awake()
    {
        base.Awake();
        _bulletContainer = new GameObject("UfoBulletContainer").transform;
        _bulletPool = new Pool<UfoBullet>(_bulletPrefab, _bulletPoolSize, _bulletContainer);
    }

    private void OnEnable()
    {
        EventManager.OnNewGame.AddListener(NewGame);
    }

    protected override void Update()
    {
        CheckIntersectionWithBorders();
        UfoMove();
        UfoFire();
    }

    private void NewGame()
    {
        _bulletPool.ReturnAllActorsToPool();
    }

    public void Init(Transform playerTransform, float speed, bool isRightMoving)
    {
        _playerTransform = playerTransform;
        _speed = speed;
        _isRightMoving = isRightMoving;
        _transform.position = GetRandonPosition();
        SetFireCooldown();
        gameObject.SetActive(true);
    }

    private Vector2 GetRandonPosition()
    {
        float inaccessibleAreaHeight = _camHalfHeight * 2 / 100 * INACCESSIBLE_AREA_PERSENT;
        float height = _camHalfHeight - inaccessibleAreaHeight;
        float x = _isRightMoving ? -_camHalfWidth : _camHalfWidth;
        float y = Random.Range(-height, height);

        return new Vector2(x, y);
    }

    private void UfoMove()
    {
        Vector3 direction = _isRightMoving ? Vector3.right : Vector3.left;
        _transform.Translate(_speed * Time.deltaTime * direction);
    }

    private void UfoFire()
    {
        _fireLastTime -= Time.deltaTime;

        if (_fireLastTime <= 0)
        {
            SetFireCooldown();
            UfoBullet bullet = _bulletPool.GetFreeActor(true);
            bullet.Init(_transform.position, _playerTransform.position);
            EventManager.SendPlaySound(SoundType.Fire);
        }
    }

    private void SetFireCooldown()
    {
        _fireLastTime = Random.Range(MIN_FIRE_COOLDOWN, MAX_FIRE_COOLDOWN);
    }

    public void KillEnemy(bool isByPlayer = true)
    {
        EventManager.SendUfoDestruction(isByPlayer);
        ReturnToPool();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            KillEnemy(false);
            player.KillPlayer();
        }
    }
}
