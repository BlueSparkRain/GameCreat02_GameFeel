using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ֵؿ鸽�Ӽ���
//�ռ�����飬
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
