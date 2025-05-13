using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RemoveCommand : SquareCommand
{
    public S_RemoveCommand(Square square) : base(square)
    {
    }

    public override void Excute()
    {
        base.Excute();
        Remove();
    }

    void Remove() 
    {
        controlSquare.BeRemoved();
    
    }


}
