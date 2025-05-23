using System.Collections;
using UnityEngine;

/// <summary>
/// 特殊方块
/// </summary>
public class SpecicalSquare : Square
{
    [Header("特殊地块精灵")]
    public Sprite SpecicalSprite;

    [Header("本地块可摧毁")]
    bool canRemoved;

    protected SubCol selfCol;
    protected GameRow selfRow;

    protected override void Awake()
    {
        base.Awake();
        MyAppear();
    }

    protected virtual void RemainSelfEffect() 
    {

    }

    /// <summary>
    /// 自身特殊形象整顿
    /// </summary>
    void MyAppear()
    {
        transform.GetComponent<SpriteRenderer>().sprite = SpecicalSprite;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void RemoveSelfEffect()
    {
      base .RemoveSelfEffect();
    }

    public override IEnumerator BeRemoved()
    {
        RemainSelfEffect();

        if (!canRemoved)
        {
            RemainSelfEffect();
            yield break;
        }

        yield return base.BeRemoved();

    }
   
}

public enum E_SpecialSquareType 
{   
    消融方块,
    传送门方块,

}
