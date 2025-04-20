using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class S_ColorCommand : ColorSquareCommand
{
    public S_ColorCommand(ColorSquare square,  SquareGroup squareGroup) : base(square, squareGroup)
    {

    }
    public override void Excute()
    {
        base.Excute();
        ColoeSelf();
    }


    void ColoeSelf() 
    {
        colorSquare.ColorSelf();
    }

}
