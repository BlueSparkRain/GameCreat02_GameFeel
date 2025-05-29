using System.Collections;
using UnityEngine;

/// <summary>
/// 区域检测可消除方块
/// </summary>
public class CollectableSquare : SpecicalSquare
{
    public E_Collectable type;

    //[Header("需要消除的次数")]
    //public int removeTime = 1;

    [Header("附近消除检测距离")]
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
    /// 可收集方块周围消除检测，可接收距离3（2.9）为临界最大值
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
        //消除后锁定上槽+本位生成一个方块
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

        poolManager.ObjReturnPool(E_ObjectPoolType.收集块池, gameObject);
    }
}
