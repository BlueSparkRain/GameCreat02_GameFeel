using System.Collections;
using UnityEngine;

/// <summary>
/// 可收集（特殊）方块
/// </summary>
public class CollectableSquare : SpecicalSquare
{
    private BoxCollider2D checkAera;
    public E_Collectable type;

    bool canTrigger=true;

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
        Debug.Log(Vector2.Distance(square.position, transform.position));
        if (canTrigger && Vector2.Distance(square.position, transform.position) <= 3)
        {
            canTrigger = false;
            StartCoroutine(BeRemoved());
        }
    }


    public override IEnumerator BeRemoved()
    {
        yield return base.BeRemoved();
        FindAnyObjectByType<CollectablesRecorder>().GetCollectable(type);//记录收集物
        if (transform.parent && transform.parent.parent.GetComponent<SquareColumn>())
        {
            transform?.parent.parent.GetComponent<SquareColumn>().AddMaxSpawnNum();//恢复列最大容量
            yield return transform?.parent.GetComponent<Slot>().ThrowSquare();
            //粒子特效
            //音效
            yield return new WaitForSeconds(0.2f);
            DestroyImmediate(gameObject);
        }
    }


}
