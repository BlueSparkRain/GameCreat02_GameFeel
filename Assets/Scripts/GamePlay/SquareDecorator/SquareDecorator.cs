using System.Diagnostics;
using UnityEditor;

public abstract class SquareDecorator : ISpecialPower
{
    public ISpecialPower iAmSpecial;
    public SquareDecorator(ISpecialPower power) 
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
