using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSquareCommand : SquareCommand
{
    protected ColorSquare colorSquare;
    public ColorSquareCommand(ColorSquare square, SquareGroup squareGroup) : base(square,squareGroup)
    {
        colorSquare = square;
    }

}
