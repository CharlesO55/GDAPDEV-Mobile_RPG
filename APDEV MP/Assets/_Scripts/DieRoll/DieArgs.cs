public class DieArgs
{
    private int _minValue;
    private bool _rollPass;
    private int _rolledValue;

    //ACCESS
    public int MinValue
    {
        get { return _minValue; }
    }
    public int RolledValue
    {
        get { return _rolledValue; }
    }
    public bool RollPass
    {
        get { return _rollPass; }
    }

    public DieArgs(int minValue, int rolledValue, bool rollPass)
    {
        _minValue = minValue;
        _rolledValue = rolledValue;
        _rollPass = rollPass;
    }
}
