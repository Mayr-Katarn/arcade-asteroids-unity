using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [Space(6), Header("Asteroids config")]
    [SerializeField] private Asteroid _asteroidPrefab;
    [SerializeField] private int _astroidPoolSize = 20;
    [SerializeField] private int _startAsteroidsCount = 2;
    [SerializeField] private float _minAsteroidSpeed = 0.5f;
    [SerializeField] private float _maxAsteroidSpeed = 2;
    [SerializeField] private float _newAsteroidsFlyawayAngle = 45;
    [Space(6), Header("UFO config")]
    [SerializeField] private Ufo _ufoPrefab;
    [SerializeField] private int _ufoPoolSize = 1;
    [SerializeField] private float _ufoSpeed = 1;
    [SerializeField] private float _minUfoCooldown = 20;
    [SerializeField] private float _maxUfoCooldown = 40;

    private Transform _asteroidContainer;
    private Pool<Asteroid> _asteroidPool;
    private Transform _ufoContainer;
    private Pool<Ufo> _ufoPool;
    private int _level = 0;
    private int _newAsteroidsWaveCounter = 0;
    private float _playerSafeZoneSize = 1;
    private float _camHalfHeight;
    private float _camHalfWidth;
    private bool _isUfoActive = true;
    private float _ufoCooldown;
    private const int NEW_LEVEL_COOLDOWN = 2;
    private const int BIG_ASTEROID_COAST = 4;
    private const int MIDDLE_ASTEROID_COAST = 2;
    private const int SMALL_ASTEROID_COAST = 1;

    private void Awake()
    {
        _camHalfHeight = Camera.main.orthographicSize;
        _camHalfWidth = Camera.main.aspect * _camHalfHeight;
        _playerSafeZoneSize = _camHalfWidth * 2 / 100 * 10;
    }

    private void OnEnable()
    {
        EventManager.OnNewGame.AddListener(NewGame);
        EventManager.OnAsteroidDestruction.AddListener(AsteroidDestruction);
        EventManager.OnUfoDestruction.AddListener(UfoDestruction);
    }

    private void Start()
    {
        CreateAsteroidPoll();
        CreateUfoPool();
    }

    private void Update()
    {
        CheckUfoSpawn();
    }

    private void NewGame()
    {
        _level = 0;
        StopAllCoroutines();
        _asteroidPool.ReturnAllActorsToPool();
        _ufoPool.ReturnAllActorsToPool();
        StartNewLevel();
        StartUfoCooldown();
    }

    private void CreateAsteroidPoll()
    {
        _asteroidContainer = new GameObject("AsteroidContainer").transform;
        _asteroidContainer.SetParent(transform);
        _asteroidPool = new Pool<Asteroid>(_asteroidPrefab, _astroidPoolSize, _asteroidContainer);
    }

    private void CreateUfoPool()
    {
        _ufoContainer = new GameObject("UfoContainer").transform;
        _ufoContainer.SetParent(transform);
        _ufoPool = new Pool<Ufo>(_ufoPrefab, _ufoPoolSize, _ufoContainer);
    }

    private void StartNewLevel()
    {
        int asteroidsCounter = _startAsteroidsCount + _level;
        _newAsteroidsWaveCounter = asteroidsCounter * BIG_ASTEROID_COAST;

        for (int i = 0; i < asteroidsCounter; i++)
        {
            CreateAsteroid();
        }
    }

    private void StartUfoCooldown()
    {
        _ufoCooldown = Random.Range(_minUfoCooldown, _maxUfoCooldown);
        _isUfoActive = false;
    }

    private void CheckUfoSpawn()
    {
        _ufoCooldown -= Time.deltaTime;

        if (!_isUfoActive && _ufoCooldown <= 0)
        {
            CreateUfo();
        }
    }

    private void CreateAsteroid()
    {
        Asteroid asteroid = _asteroidPool.GetFreeActor(true);
        asteroid.Init(GetRandomAsteroidPosition(), GetRandomAsteroidAngle(), GetRandomAsteroidSpeed());
    }

    private void CreateAsteroid(Vector2 position, Vector3 angle, float speed, AsteroidType asteroidType)
    {
        Asteroid asteroid = _asteroidPool.GetFreeActor(true);
        asteroid.Init(position, angle, speed, asteroidType);
    }

    private void CreateUfo()
    {
        _isUfoActive = true;
        Ufo ufo = _ufoPool.GetFreeActor();
        ufo.Init(_playerTransform, _ufoSpeed, Random.Range(0, 2) == 0);
    }

    private Vector2 GetRandomAsteroidPosition()
    {
        float leftMinX = -_camHalfWidth;
        float leftMaxX = _playerTransform.position.x - _playerSafeZoneSize;
        float rightMinX = _playerTransform.position.x + _playerSafeZoneSize;
        float rightMaxX = _camHalfWidth;

        if (leftMinX > leftMaxX)
        {
            leftMinX = rightMinX;
            leftMaxX = rightMaxX;
        }
        else if (rightMinX > rightMaxX)
        {
            rightMinX = leftMinX;
            rightMaxX = leftMaxX;
        }

        float botMinY = -_camHalfHeight;
        float botMaxY = _playerTransform.position.y - _playerSafeZoneSize;
        float topMinY = _playerTransform.position.y + _playerSafeZoneSize;
        float topMaxY = _camHalfHeight;

        if (botMinY > botMaxY)
        {
            botMinY = topMinY;
            botMaxY = topMaxY;
        }
        else if (topMinY > topMaxY)
        {
            topMinY = botMinY;
            topMaxY = botMaxY;
        }

        float x = Random.Range(0, 2) == 0 ? Random.Range(leftMinX, leftMaxX) : Random.Range(rightMinX, rightMaxX);
        float y = Random.Range(0, 2) == 0 ? Random.Range(botMinY, botMaxY) : Random.Range(topMinY, topMaxY);
        return new Vector2(x, y);
    }

    private Vector3 GetRandomAsteroidAngle()
    {
        return Vector3.forward * Random.Range(0, 361);
    }

    private float GetRandomAsteroidSpeed()
    {
        return Random.Range(_minAsteroidSpeed, _maxAsteroidSpeed);
    }

    private void AsteroidDestruction(AsteroidType asteroidType, Transform oldTransform, bool isByPlayer)
    {
        EventManager.SendPlaySound(SoundType.Explosion);

        if (isByPlayer)
        {
            switch (asteroidType)
            {
                case AsteroidType.AsteroidSmall:
                    _newAsteroidsWaveCounter -= SMALL_ASTEROID_COAST;
                    break;

                case AsteroidType.AsteroidMiddle:
                case AsteroidType.AsteroidBig:
                    Vector2 position = oldTransform.position;
                    Vector3 oldAngle = oldTransform.eulerAngles;
                    Vector3 bigerAngle = Vector3.forward * (oldAngle.z + _newAsteroidsFlyawayAngle);
                    Vector3 lessAngle = Vector3.forward * (oldAngle.z - _newAsteroidsFlyawayAngle);
                    Vector3[] newAngles = { bigerAngle, lessAngle };
                    float speed = GetRandomAsteroidSpeed();

                    for (int i = 0; i < 2; i++)
                    {
                        CreateAsteroid(position, newAngles[i], speed, asteroidType - 1);
                    }
                    break;

                default:
                    break;
            }

            GameConfig.SetScore((EnemyType)asteroidType);

        }
        else
        {
            switch (asteroidType)
            {
                case AsteroidType.AsteroidSmall:
                    _newAsteroidsWaveCounter -= SMALL_ASTEROID_COAST;
                    break;

                case AsteroidType.AsteroidMiddle:
                    _newAsteroidsWaveCounter -= MIDDLE_ASTEROID_COAST;
                    break;

                case AsteroidType.AsteroidBig:
                    _newAsteroidsWaveCounter -= BIG_ASTEROID_COAST;
                    break;
            }
        }

        if (_newAsteroidsWaveCounter == 0)
        {
            _level++;
            StartCoroutine(NewLevelCooldown());
        }
    }

    private void UfoDestruction(bool isByPlayer)
    {
        StartUfoCooldown();
        EventManager.SendPlaySound(SoundType.Explosion);

        if (isByPlayer)
            GameConfig.SetScore(EnemyType.Ufo);
    }

    private IEnumerator NewLevelCooldown()
    {
        yield return new WaitForSeconds(NEW_LEVEL_COOLDOWN);
        StartNewLevel();
    }
}
