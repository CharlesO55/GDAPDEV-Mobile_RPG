public class DieArgs
{
    private int _minValue;
    public int MinValue
    {
        get { return _minValue; }
    }

    private int _rolledValue;
    public int RolledValue
    {
        get { return _rolledValue; }
    }

    private bool _RollPass;
    public bool RollPass
    {
        get { return _RollPass; }
    }

    public DieArgs(int minValue, int rolledValue, bool rollPass)
    {
        _minValue = minValue;
        _rolledValue = rolledValue;
        _RollPass = rollPass;
    }
}
