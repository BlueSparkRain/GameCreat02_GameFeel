using System.Diagnostics;
using UnityEditor;

public class SquareDecorator : IHaveSpecialPower
{
    public IHaveSpecialPower iAmSpecial;
    public SquareDecorator(IHaveSpecialPower power) 
    {
    iAmSpecial = power;
    }

    public virtual  void PowerInit()
    {
        iAmSpecial.PowerInit();
    }

    public virtual void PowerOnUpdate()
    {
        iAmSpecial.PowerOnUpdate();
    }

    public virtual void TriggerPower()
    {
        iAmSpecial.TriggerPower();
    }
}
