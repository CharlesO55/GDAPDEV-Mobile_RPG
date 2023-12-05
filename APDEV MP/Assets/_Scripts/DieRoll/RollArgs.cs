using UnityEngine;

public class RollArgs 
{
    public readonly int MinRollValue;
    public readonly bool IsInstantaneousRoll;
    public readonly Vector3 DropPosition;
    public readonly Vector3 ThrowDirection;

    public RollArgs(int minRollValue, bool isInstantaneousRoll, Vector3 dropPosition, Vector3 throwDirection)
    {
        this.MinRollValue = minRollValue;
        this.IsInstantaneousRoll = isInstantaneousRoll;
        this.DropPosition = dropPosition;
        this.ThrowDirection = throwDirection;
    }
}
