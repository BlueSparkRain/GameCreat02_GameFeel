using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRemovableSquare : SpecicalSquare
{
    bool canTrigger = true;

    [Header("������������")]
    public int RemoveIndex = 0;

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_TaskTrigger,TriggerRemove);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_TaskTrigger,TriggerRemove);
    }

    void TriggerRemove(int triggerBitch) 
    {
        SetSquareRemovable(true);
        if (triggerBitch == RemoveIndex) 
        {
            EventRemoveSelf();
        }
    }


    void EventRemoveSelf()
    {
        if (canTrigger)
        {
            canTrigger = false;
            StartCoroutine(BeRemoved());
        }
    }

    protected override void RemainSelfEffect()
    {
        base.RemainSelfEffect();
    }

    public override void RemoveSelfEffect()
    {
        base.RemoveSelfEffect();
    }

    public override IEnumerator BeRemoved()
    {
        if (!canRemoved)
        {
            Debug.Log("��ǰ�����޷�������");
            yield break;
        }

        selfCol ??= GetComponentInParent<SubCol>();
        yield return base.BeRemoved();

        if (transform.parent != null && slot)
        {
            yield return SquareRemoveAnim();
            transform.SetParent(null);
            slot.ThrowSquare();
        }
        selfCol.GetTargetSlotNewSquare(slot);

        poolManager.ObjReturnPool(E_ObjectPoolType.�ռ����, gameObject);
    }
}
