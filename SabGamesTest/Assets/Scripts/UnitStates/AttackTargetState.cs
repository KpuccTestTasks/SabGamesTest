public class AttackTargetState : UnitState
{
    public AttackTargetState(Unit unit) : base(unit)
    { }
    
    public override void Update()
    {
        if (Unit.CurrentTarget == null)
        {
            Unit.SetState(new LookingForTargetState(Unit));
            return;
        }
        
        if (Unit.TimeToNextAttack <= 0)
            Unit.AttackCurrentTarget();
    }
}
