using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Actor
{
    [SerializeField] private SpriteRenderer _ship;
    [SerializeField] private float _invulnerabilityTime = 3f;

    private EdgeCollider2D _collider;
    private int _playerLives;
    private const float BLINK_TIME = 0.5f;
    private const float DESTRUCTION_COOLDOWN = 1f;

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<EdgeCollider2D>();
    }

    private void OnEnable()
    {
        EventManager.OnNewGame.AddListener(NewGame);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void NewGame()
    {
        StopAllCoroutines();
        _playerLives = GameConfig.StartPlayerLives;
        _transform.position = Vector2.zero;
        _collider.enabled = false;
        gameObject.SetActive(true);
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (_invulnerabilityTime / BLINK_TIME % 2 == 0) 
            _ship.enabled = false;

        StartCoroutine(SpawnInvulnerability());
    }

    public void KillPlayer()
    {
        _playerLives--;
        StartCoroutine(ShipDestoyed());
        EventManager.SendPlayerKill(_playerLives);
        EventManager.SendPlaySound(SoundType.Explosion);

        if (_playerLives == 0)
        {
            EventManager.SendGameOver();
            gameObject.SetActive(false);
        }
        else
        {
            SpawnPlayer();
        }
    }

    private IEnumerator SpawnInvulnerability()
    {
        for (int i = 0; i < _invulnerabilityTime / BLINK_TIME; i++)
        {
            _ship.enabled = !_ship.enabled;
            yield return new WaitForSeconds(BLINK_TIME);
        }

        _ship.enabled = true;
        _collider.enabled = true;
    }

    private IEnumerator ShipDestoyed()
    {
        _collider.enabled = false;
        _ship.enabled = false;
        yield return new WaitForSeconds(DESTRUCTION_COOLDOWN);
    }
}
