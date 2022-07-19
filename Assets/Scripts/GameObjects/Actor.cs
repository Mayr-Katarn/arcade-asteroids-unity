using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected Transform _transform;
    protected Camera _camera;
    protected float _camHalfHeight;
    protected float _camHalfWidth;

    protected virtual void Awake()
    {
        _transform = transform;
        _camera = Camera.main;
        _camHalfHeight = _camera.orthographicSize;
        _camHalfWidth = _camera.aspect * _camHalfHeight;
    }

    protected virtual void Update()
    {
        CheckIntersectionWithBorders();
    }

    protected virtual void CheckIntersectionWithBorders()
    {
        float x = _transform.position.x;
        float y = _transform.position.y;

        if (x > _camHalfWidth)
        {
            _transform.position = new Vector2(-_camHalfWidth, y);
        }
        else if (x < -_camHalfWidth)
        {
            _transform.position = new Vector2(_camHalfWidth, y);
        }

        if (y > _camHalfHeight)
        {
            _transform.position = new Vector2(x, -_camHalfHeight);
        }
        else if (y < -_camHalfHeight)
        {
            _transform.position = new Vector2(x, _camHalfHeight);
        }
    }

    protected virtual void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
