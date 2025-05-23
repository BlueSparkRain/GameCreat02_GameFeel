using System.Collections;
using UnityEngine;

/// <summary>
/// ���ⷽ��
/// </summary>
public class SpecicalSquare : Square
{
    [Header("����ؿ龫��")]
    public Sprite SpecicalSprite;

    [Header("���ؿ�ɴݻ�")]
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
    /// ����������������
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
    ���ڷ���,
    �����ŷ���,

}
