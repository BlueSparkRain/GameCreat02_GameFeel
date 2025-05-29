using System.Collections;
using UnityEngine;

/// <summary>
/// 特殊方块
/// </summary>
public class SpecicalSquare : Square
{
    public SpecialSquareSO   specialData;

    //[Header("本地块可摧毁")]
    //bool canPlayerRemoved;

    protected SubCol selfCol;
    protected GameRow selfRow;

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 自身特殊形象整顿
    /// </summary>
    public virtual void InitSpecialSquare(SpecialSquareSO _specialData)
    {
        specialData = _specialData;
        transform.GetComponent<SpriteRenderer>().sprite = specialData.specialSprite;
    }

    /// <summary>
    /// 与消除无关的逻辑
    /// </summary>
    protected virtual void RemainSelfEffect()
    {

    }

    /// <summary>
    /// 与消除有关的逻辑
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
    消融收集,
    触发消除,
    传送,
}
