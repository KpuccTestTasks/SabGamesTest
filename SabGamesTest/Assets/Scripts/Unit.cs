using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private int _hp;
    private int _attackPower;

    private bool _isMooving;
    private float _moveTime;
    private float _totalMoveTime;
    private Vector3 _startPosition;
    private Vector3 _moveTarget;

    private const float MoveSpeed = 8f;

    private void Update()
    {
        if (_isMooving)
        {
            _moveTime += Time.deltaTime;
            float interpolatedTime = 1 - (_totalMoveTime - _moveTime) / _totalMoveTime;
            
            transform.position = Vector3.Lerp(_startPosition, _moveTarget, interpolatedTime);

            var distanceToTarget = (_moveTarget - transform.position).sqrMagnitude;

            if (distanceToTarget < 0.5f)
                _isMooving = false;
        }
    }

    public void Move(Vector3 targetPos)
    {
        _startPosition = transform.position;
        _moveTarget = targetPos;

        _moveTime = 0f;
        _totalMoveTime = (_moveTarget - _startPosition).magnitude / MoveSpeed;
        
        _isMooving = true;
    }
}
