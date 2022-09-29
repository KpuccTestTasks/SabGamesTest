using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    [SerializeField] private Renderer unitRenderer;
    
    public int TeamId { get; private set; }
    
    private int _hp;
    private int _attackPower;
    private UnitState _state;

    public float TimeToNextAttack { get; private set; }
    public Unit CurrentTarget { get; set; }

    public const float MoveSpeed = 8f;
    
    private const float AttackCooldown = 1.5f;
    private const float AttackRadius = 1.5f;

    public static event Action<Unit> OnUnitDied;

    private void Update()
    {        
        if (TimeToNextAttack > 0)
        {
            TimeToNextAttack -= Time.deltaTime;
        }
        
        _state.Update();
    }

    public void Setup(int teamId, Color teamColor)
    {
        TeamId = teamId;
        unitRenderer.material.color = teamColor;
        
        _hp = Random.Range(10, 30);
        _attackPower = Random.Range(2, 5);
        TimeToNextAttack = 0;
        
        SetState(new LookingForTargetState(this));
    }

    public void SetState(UnitState newState)
    {
        _state = newState;
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

    public void AttackCurrentTarget()
    {
        CurrentTarget.ApplyDamage(_attackPower);
        TimeToNextAttack = AttackCooldown;
    }

    public bool IsTargetInAttackRadius()
    {
        var distanceToTarget = (CurrentTarget.transform.position - transform.position).magnitude;

        return distanceToTarget <= AttackRadius;
    }
}
