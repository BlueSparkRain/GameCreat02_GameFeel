public class S_ColorCommand : ColorSquareCommand
{
    public S_ColorCommand(ColorSquare square) : base(square)
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
