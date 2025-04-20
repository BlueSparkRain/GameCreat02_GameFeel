using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RemoveCommand : SquareCommand
{
    public S_RemoveCommand(Square square, SquareGroup squareGroup) : base(square, squareGroup)
    {
    }

    public override void Excute()
    {
        base.Excute();
        Remove();
    }

    void Remove() 
    {
        square.BeRemoved();
    
    }


}
