using System.Collections;
using Unity.Content;
using UnityEngine;

/// <summary>
/// 可收集（特殊）方块
/// </summary>
public class CollectableSquare : SpecicalSquare
{
    private BoxCollider2D checkAera;
    public E_Collectable type;
    [Header("需要消除的次数")]
    public int moveTime=1;

    bool canTrigger=true;

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckSelf);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckSelf);
    }
    bool canMinus=true;

    /// <summary>
    /// 可收集方块周围消除检测，可接收距离3（2.9）为临界最大值
    /// </summary>
    /// <param name="square"></param>
    void CheckSelf(Transform square) 
    {
        if (canTrigger && canMinus && Vector2.Distance(square.position, transform.position) <= 3)
        {
             StartCoroutine(MinusCheck());
            if (moveTime <= 0)
            {
                canTrigger = false;
                StartCoroutine(BeRemoved());
            }
        }
    }

    IEnumerator MinusCheck() 
    {
       canMinus = false; 
       moveTime -= 1;
       yield return new WaitForSeconds(1);
       canMinus = true;
    }

    public override IEnumerator BeRemoved()
    {
        yield return base.BeRemoved();
        FindAnyObjectByType<CollectablesRecorder>().GetCollectable(type);//记录收集物
        if (transform.parent && transform.parent.parent.GetComponent<SquareColumn>())
        {
            transform?.parent.parent.GetComponent<SquareColumn>().AddMaxSpawnNum();//恢复列最大容量
            transform?.parent.GetComponent<Slot>().ThrowSquare();
            //粒子特效
            //音效
            yield return new WaitForSeconds(0.2f);
            DestroyImmediate(gameObject);
        }
    }
}
