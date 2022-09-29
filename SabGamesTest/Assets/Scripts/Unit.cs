using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    public int TeamId { get; private set; }
    
    private int _hp;
    private int _attackPower;

    private bool _isMooving;

    private float _timeToNextAttack;
    private Unit _currentTarget;

    private const float MoveSpeed = 8f;
    private const float AttackCooldown = 1.5f;
    private const float AttackRadius = 1.5f;

    public static event Action<Unit> OnUnitDied;
    
    /*
     * State machine
     * 
     * Look for enemy
     * Move to enemy
     * Attack enemy
     */

    private void Update()
    {        
        if (_timeToNextAttack > 0)
        {
            _timeToNextAttack -= Time.deltaTime;
        }
        
        if (_currentTarget == null)
        {
            _currentTarget = Field.Instance.GetClosestTargetToUnit(this);

            if (_currentTarget == null)
                return;
        }
        
        var distanceToTarget = (_currentTarget.transform.position - transform.position).magnitude;
            
        if (distanceToTarget > AttackRadius)
        {
            if (!_isMooving)
            {
                Move();
            }
        }
        else if (_timeToNextAttack <= 0f)
        {
            AttackCurrentTarget();
        }
        
        if (_isMooving)
        {
            var position = transform.position;
            var direction = _currentTarget.transform.position - position;

            var newPosition = Vector3.Lerp(position, position + direction.normalized, Time.deltaTime * MoveSpeed);
            
            transform.position = newPosition;

            if (distanceToTarget <= AttackRadius)
                StopMoving();
        }
    }

    public void Setup(int teamId)
    {
        TeamId = teamId;
        
        _hp = Random.Range(10, 30);
        _attackPower = Random.Range(2, 5);
        _timeToNextAttack = 0;
    }

    private void Move()
    {
        _isMooving = true;
    }

    private void StopMoving()
    {
        _isMooving = false;
    }

    private void ApplyDamage(int damage)
    {
        _hp -= damage;

        if (_hp <= 0)
        {
            OnUnitDied?.Invoke(this);
            
            Destroy(gameObject);
        }
    }

    private void AttackCurrentTarget()
    {
        _currentTarget.ApplyDamage(_attackPower);
        _timeToNextAttack = AttackCooldown;
    }
}
