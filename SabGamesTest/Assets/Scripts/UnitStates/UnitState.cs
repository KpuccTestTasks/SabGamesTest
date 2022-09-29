public abstract class UnitState
{
    protected Unit Unit { get; }
    public abstract void Update();

    public UnitState(Unit unit)
    {
        Unit = unit;
    }
}
