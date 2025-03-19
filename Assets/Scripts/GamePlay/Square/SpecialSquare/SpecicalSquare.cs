using System.Collections;
using UnityEngine;

/// <summary>
/// 特殊方块
/// </summary>
public class SpecicalSquare : Square, ICanSpecical
{
    [Header("特殊地块精灵")]
    public Sprite SpecicalSprite;

    [Header("本地块可摧毁")]
    bool canRemoved;

    protected override void Awake()
    {
        base.Awake();
        MyAppear();
    }

    /// <summary>
    /// 自身特殊形象整顿
    /// </summary>
    void MyAppear()
    {
        transform.GetComponent<SpriteRenderer>().sprite = SpecicalSprite;
    }

    public override void DoSelfEffect()
    {
     
    }

    public override IEnumerator BeRemoved()
    {
        yield return base.BeRemoved();
        if(!canRemoved)
            yield break;

    }

    public virtual void DoSelfSpecical()
    {
        Debug.Log("特殊地块特殊功能");
    }

   
}
