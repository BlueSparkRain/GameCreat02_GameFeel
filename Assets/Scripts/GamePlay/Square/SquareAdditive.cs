using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//各种地块附加技能
//收集物，冰块，
public class SquareAdditive : ColorSquare,ICanEffect
{
    public Sprite AdditiveSprite;
    private ICanEffect selfStrategy;

    public void SelectStrategy(ICanEffect stretagy)
    {
      selfStrategy = stretagy;
    }

    public void DoExcute()
    {
      selfStrategy.DoExcute();
    }
}
