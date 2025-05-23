using System.Collections;
using UnityEngine;

/// <summary>
/// ���ռ������⣩����
/// </summary>
public class CollectableSquare : SpecicalSquare
{
    public E_Collectable type;
    [Header("��Ҫ�����Ĵ���")]
    public int moveTime = 1;

    bool canTrigger = true;

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckSelf);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckSelf);
    }
    bool canMinus = true;

    /// <summary>
    /// ���ռ�������Χ������⣬�ɽ��վ���3��2.9��Ϊ�ٽ����ֵ
    /// </summary>
    /// <param name="square"></param>
    void CheckSelf(Transform square)
    {
        if (canTrigger && canMinus && Vector2.Distance(square.position, transform.position) <= 3)
        {
            StartCoroutine(AeraCheck());
            if (moveTime <= 0)
            {
                canTrigger = false;
                StartCoroutine(BeRemoved());
            }
        }
    }

    public override void RemoveSelfEffect()
    {
        base.RemoveSelfEffect();
        //�����������ϲ�+��λ����һ������
    }


    protected override void RemainSelfEffect()
    {
        base.RemainSelfEffect();


    }

    IEnumerator AeraCheck()
    {
        canMinus = false;
        moveTime -= 1;
        yield return new WaitForSeconds(1);
        canMinus = true;
    }

    public override IEnumerator BeRemoved()
    {
        yield return base.BeRemoved();

        if (transform.parent != null && slot)
        {
            yield return SquareRemoveAnim();
            transform.SetParent(null);
            slot.ThrowSquare();
        }

        selfCol.GetTargetSlotNewSquare(slot);
    }
}
