using UnityEngine;

public class SquareSuperMarkDecorator :SquareDecorator
{
    public SquareSuperMarkDecorator(IHaveSpecialPower power) : base(power)
    {
    }

    public override void PowerInit()
    {
        base.PowerInit();
    }

    public override void PowerOnUpdate()
    {
        base.PowerOnUpdate();
    }

    public override void TriggerPower()
    {
        base.TriggerPower();
    }
}
