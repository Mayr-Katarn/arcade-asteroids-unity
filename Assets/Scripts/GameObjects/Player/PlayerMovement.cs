using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _keyBoardRotateSpeed = 1;
    [SerializeField] private float _mouseRotateSpeed = 1;
    [SerializeField] private float _maxSpeed = 10;
    [SerializeField] private float _acceleration = 1;

    private Transform _transform;
    private float _speed = 0;
    private Vector3 _currendDirection = Vector3.zero;
    private Vector3 _lastDirection = Vector3.zero;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        EventManager.OnNewGame.AddListener(NewGame);
    }

    public Vector3 m;

    private void Update()
    {
        PlayerMove();
        PlayerRotation();
        m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void NewGame()
    {
        _currendDirection = Vector3.zero;
        _lastDirection = Vector3.zero;
        _transform.eulerAngles = Vector3.zero;
    }

    private void PlayerMove()
    {
        float input = Input.GetAxis("Forward");

        if (input != 0)
        {
            EventManager.SendPlaySound(SoundType.Thruster);

            if (_speed < _maxSpeed)
            {
                _speed += _acceleration * Time.deltaTime;
                if (_speed > _maxSpeed) _speed = _maxSpeed;
            }

            _currendDirection = _transform.up * _speed;

            if (_lastDirection != Vector3.zero)
            {
                _lastDirection = Vector3.Lerp(_lastDirection, Vector3.zero, _acceleration * Time.deltaTime);
                if (_lastDirection.magnitude < 0.1) _lastDirection = Vector3.zero;
            }
        }
        else
        {
            EventManager.SendStopPlayThruster();
            _speed = 0;

            if (_currendDirection != Vector3.zero)
            {
                _lastDirection += _currendDirection;
                _currendDirection = Vector3.zero;
            }
        }

        if (!GameConfig.isGamePaused)
            _transform.position += GetImpulse();
    }

    private Vector3 GetImpulse()
    {
        return (_lastDirection + _currendDirection) / 100;
    }

    private void PlayerRotation()
    {
        switch (GameConfig.InputType)
        {
            case InputType.Keyboard:
                KeyboardRotation();
                break;

            case InputType.MouseKeyboard:
                MouseRotation();
                break;

            default:
                KeyboardRotation();
                break;
        }
    }

    private void KeyboardRotation()
    {
        float input = Input.GetAxis("Horizontal");

        float x = _transform.eulerAngles.x;
        float y = _transform.eulerAngles.y;
        float z = _transform.eulerAngles.z + input * Time.deltaTime * _keyBoardRotateSpeed;
        _transform.eulerAngles = new Vector3(x, y, z);
    }

    private void MouseRotation()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 targetDirection = mousePosition - (Vector2)_transform.position;
        targetDirection.Normalize();
        _transform.up = Vector2.Lerp(_transform.up, targetDirection, Time.deltaTime * _mouseRotateSpeed);
    }
}
