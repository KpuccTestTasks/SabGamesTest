public class LookingForTargetState : UnitState
{
    public LookingForTargetState(Unit unit) : base(unit)
    { }
    
    public override void Update()
    {
        if (Unit.CurrentTarget != null)
        {
            if (Unit.IsTargetInAttackRadius())
            {
                Unit.SetState(new AttackTargetState(Unit));
            }
            else
            {
                Unit.SetState(new MoveToTargetState(Unit));
            }

            return;
        }

        var target = Field.Instance.GetClosestTargetToUnit(Unit);
        Unit.CurrentTarget = target;
    }
}
