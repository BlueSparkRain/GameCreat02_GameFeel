using System.Collections;
using UnityEngine;

/// <summary>
/// ���ⷽ��
/// </summary>
public class SpecicalSquare : Square
{
    public SpecialSquareSO   specialData;

    //[Header("���ؿ�ɴݻ�")]
    //bool canPlayerRemoved;

    protected SubCol selfCol;
    protected GameRow selfRow;

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// ����������������
    /// </summary>
    public virtual void InitSpecialSquare(SpecialSquareSO _specialData)
    {
        specialData = _specialData;
        transform.GetComponent<SpriteRenderer>().sprite = specialData.specialSprite;
    }

    /// <summary>
    /// �������޹ص��߼�
    /// </summary>
    protected virtual void RemainSelfEffect()
    {

    }

    /// <summary>
    /// �������йص��߼�
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
    �����ռ�,
    ��������,
    ����,
}
