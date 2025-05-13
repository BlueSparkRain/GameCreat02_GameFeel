using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按十字线行动
/// </summary>
public class AI_TenMoveCommand : AISquare_Command
{
    public override void Excute()
    {
        base.Excute();
        MoveOnce();
    }
    void MoveOnce() 
    {
    
    }
}
