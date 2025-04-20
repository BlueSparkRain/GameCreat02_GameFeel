public class S_LooseCommand : SquareCommand
{
    public S_LooseCommand(Square square, SquareGroup squareGroup) : base(square, squareGroup){ 
    }

    public override void Excute()
    {
        base.Excute();
        LooseSelf();
    }

    public void LooseSelf()
    {
      square.LooseSelf();
    }
}
