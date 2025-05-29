using System.Collections;
using UnityEngine;

/// <summary>
/// ���������������
/// </summary>
public class CollectableSquare : SpecicalSquare
{
    public E_Collectable type;

    //[Header("��Ҫ�����Ĵ���")]
    //public int removeTime = 1;

    [Header("��������������")]
    public float checkRemoveDistance=1;

    //bool canTrigger = true;
    public override void InitSpecialSquare(SpecialSquareSO _specialData)
    {
        base.InitSpecialSquare(_specialData);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,checkRemoveDistance);
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckSelf);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckSelf);
    }
    /// <summary>
    /// ���ռ�������Χ������⣬�ɽ��վ���3��2.9��Ϊ�ٽ����ֵ
    /// </summary>
    /// <param name="square"></param>
    void CheckSelf(Transform square)
    {
        if (canminusHealth && Vector2.Distance(square.position, transform.position) <= checkRemoveDistance)
        {
            StartCoroutine(AeraCheck());
            if (removeTime <= 0)
            {
                canminusHealth = false;
                StartCoroutine(BeRemoved());
            }
        }
    }

    void EventRemoveSelf() 
    {

        if (removeTime<=0) 
        {
            canminusHealth = false;
            StartCoroutine(BeRemoved());
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
        canminusHealth = false;
        removeTime -= 1;
        yield return new WaitForSeconds(0.3f);
        canminusHealth = true;
    }

    public override IEnumerator BeRemoved()
    {
        selfCol ??= GetComponentInParent<SubCol>();
        slot ??= GetComponentInParent<WalkableSlot>();
        yield return base.BeRemoved();

        if (transform.parent != null && slot)
        {
            yield return SquareRemoveAnim();
            transform.SetParent(null);
            slot.ThrowSquare();
        }
        selfCol ??= GetComponentInParent<SubCol>();
        slot ??= GetComponentInParent<WalkableSlot>();
        selfCol.GetTargetSlotNewSquare(slot);

        poolManager.ObjReturnPool(E_ObjectPoolType.�ռ����, gameObject);
    }
}
