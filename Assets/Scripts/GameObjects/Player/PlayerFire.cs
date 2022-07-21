using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private Transform _gunTransform;
    [SerializeField] private int _bulletPoolSize = 4;
    [SerializeField] private PlayerBullet _bulletPrefab;

    private Transform _bulletContainer;
    private Pool<PlayerBullet> _bulletPool;
    private readonly float _fireCooldown = 0.33f;
    private float _fireLastTime = 0;

    private void Awake()
    {
        _bulletContainer = new GameObject("PlayerBulletContainer").transform;
        _bulletPool = new Pool<PlayerBullet>(_bulletPrefab, _bulletPoolSize, _bulletContainer);
    }

    private void OnEnable()
    {
        EventManager.OnNewGame.AddListener(NewGame);
    }

    private void Update()
    {
        Fire();
    }

    private void NewGame()
    {
        _bulletPool.ReturnAllActorsToPool();
        _fireLastTime = 0;
    }

    private void Fire()
    {
        _fireLastTime -= Time.deltaTime;

        if (Input.GetButtonDown("Fire") && _fireLastTime <= 0 && _bulletPool.HasFreeActor(out PlayerBullet bullet, false) && !GameConfig.isGamePaused)
        {
            _fireLastTime = _fireCooldown;
            bullet.Init(_gunTransform);
            EventManager.SendPlaySound(SoundType.Fire);
        }
    }
}
