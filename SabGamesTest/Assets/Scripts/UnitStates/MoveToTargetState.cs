using UnityEngine;

public class MoveToTargetState : UnitState
{
    public MoveToTargetState(Unit unit) : base(unit)
    { }
    
    public override void Update()
    {
        var unitTarget = Unit.CurrentTarget;

        if (unitTarget == null)
        {
            Unit.SetState(new LookingForTargetState(Unit));
            return;
        }
        
        var unitPosition = Unit.transform.position;
        var unitTargetPosition = unitTarget.transform.position;
        
        var direction = unitTargetPosition - unitPosition;
        
        Unit.transform.position = Vector3.Lerp(unitPosition,
            unitPosition + direction.normalized,
            Time.deltaTime * Unit.MoveSpeed);

        if (Unit.IsTargetInAttackRadius())
            Unit.SetState(new AttackTargetState(Unit));
    }
}
