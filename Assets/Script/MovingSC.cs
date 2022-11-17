using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSC : MonoBehaviour
{
    public Vector3 EndPosition;
    public float Speed = 1f;

    private Vector3 _originalPos;
    private float _animProgress = 0f;
    private float _duration;
    private bool _forward = true;
    private Vector3 _lMovement;
    // Start is called before the first frame update
    void Start()
    {
        _originalPos = transform.position;
        float pathLenght = (EndPosition - _originalPos).magnitude;
        _duration = pathLenght / Speed;
    }

    private void FixedUpdate()
    {
        _animProgress += _forward ? Time.fixedDeltaTime : -Time.fixedDeltaTime;
        if(_animProgress > _duration)
        {
            _forward = false;
            _animProgress -= _animProgress % _duration;
        }
        else if(_animProgress < 0)
        {
            _forward = true;
            _animProgress *= -1;
        }
        SetPosition();
    }

    private void SetPosition()
    {
        float progress = _animProgress / _duration;
        Vector3 lPos = transform.position;
        Vector3 newPos = Vector3.Lerp(_originalPos, EndPosition, progress);
        transform.position = newPos;
        _lMovement = transform.position - lPos;
    }

    public Vector3 GetMovement()
    {
        return _lMovement;
    }
}
