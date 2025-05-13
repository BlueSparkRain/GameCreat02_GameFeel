using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovePower : MovePower
{
    public RandomMovePower(SquareController _squareController,  float _moveInterval = 1, float _awakePrepareTime = 2) : base(_squareController,  _moveInterval, _awakePrepareTime)
    {
    }

    protected override void FindPathMove()
    {
        base.FindPathMove();
    }
}
