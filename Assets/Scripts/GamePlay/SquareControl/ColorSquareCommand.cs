public class ColorSquareCommand : SquareCommand
{
    protected ColorSquare colorSquare;
    public ColorSquareCommand(ColorSquare square) : base(square)
    {
        colorSquare = square;
    }
}
