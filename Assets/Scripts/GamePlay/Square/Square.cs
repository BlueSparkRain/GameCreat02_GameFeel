using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Square : MonoBehaviour,ICanEffect
{
    Rigidbody2D rb;
    [HideInInspector]
    public bool HasFather;
    [Header("基础得分")]
    public int  BaseScore=50;

    [Header("本地块可以移动")]
    public bool canMove=true;

    private GameObject particalPrefab => Resources.Load<GameObject>("Prefab/SquarePartical");


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (transform.parent && transform.parent.GetComponent<Slot>())
        {
            SetMoveToSlot(transform.parent.position);

            transform.parent.parent.parent.GetChild(transform.parent.GetSiblingIndex()).GetComponent<SquareRow>().SetRowSquare(this,transform.parent.parent.GetSiblingIndex());
        }

    }

    void SetMoveToSlot(Vector3 slotPos)
    {
        HasFather = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        Vector3 truePos = new Vector3(slotPos.x, slotPos.y, -0.1f);
        if (transform.parent != null && transform.parent.GetComponent<Slot>() != null)
        {
            if (transform.parent.GetComponent<Slot>())
            {
                Slot slot = transform.parent.GetComponent<Slot>();
                transform.parent.parent.GetComponent<SquareColumn>().UpdateColumnSquares(this, transform.parent.GetSiblingIndex());
                FindAnyObjectByType<SquareGroup>().UpdateRowSquares(transform.GetComponentInChildren<Square>(), slot.transform.parent.GetSiblingIndex(), slot.transform.GetSiblingIndex());
            }
            StartCoroutine(TweenHelper.MakeLerp(transform.position, truePos, 0.15f, val => transform.position = val));
        }
    }

    /// <summary>
    /// 使本方块移动至目标槽位置
    /// </summary>
    /// <param name="slotPos"></param>
    public virtual void MoveToSlot(Vector3 slotPos)
    {
        if (transform.parent.GetComponent<Slot>() != null && (transform.parent.childCount > 1))
        {
            transform.SetParent(null);
            return;
        }

        HasFather = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        Vector3 truePos = new Vector3(slotPos.x,slotPos.y,-0.1f);
        if (transform.parent!=null&& transform.parent.GetComponent<Slot>() != null)
        {  
            if (transform.parent.GetComponent<Slot>())
            {
                Slot slot = transform.parent.GetComponent<Slot>();
                transform.parent.parent.GetComponent<SquareColumn>().UpdateColumnSquares(this, transform.parent.GetSiblingIndex());
                FindAnyObjectByType<SquareGroup>().UpdateRowSquares(transform.GetComponentInChildren<Square>(), slot.transform.parent.GetSiblingIndex(), slot.transform.GetSiblingIndex());
            }
            StartCoroutine(TweenHelper.MakeLerp(transform.position, truePos, 0.15f, val => transform.position = val));
        }
    }
    /// <summary>
    /// 松掉本方块
    /// </summary>
    public virtual void LooseSelf()
    {
        if (GetComponent<PlayerController>() != null && GetComponent<PlayerController>().isSwaping)
            return;

        transform.SetParent(null);
        HasFather = false;
        //yield return null;
        rb.velocity = new Vector3(0, -60);
    }

    public virtual IEnumerator AnimMoveScale()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.0f, 1.0f, 1.6f), 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
    }

    public virtual IEnumerator AnimReMoveScale()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.5f, 0.7f, 1.6f), 0.1f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(0.8f,2.5f, 1.6f), 0.06f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 2.6f, 0.03f, val => transform.localScale = val);
    }

    public virtual IEnumerator BeRemoved()
    {
        yield return null;
        Debug.Log("方块被消除");
        FindAnyObjectByType<PitchTest>().DingDong();

    }

    /// <summary>
    /// 执行方块技能逻辑（加分or其他）
    /// </summary>
    public virtual void DoSelfEffect()
    {
        Debug.Log("方块效果触发");
        //加分
        EventCenter.Instance.EventTrigger(E_EventType.E_GetSquareScore,BaseScore);
        ExplodeEffect();
    }

    /// <summary>
    /// 方块自身被去除时的爆炸效果
    /// </summary>
    protected virtual void ExplodeEffect() 
    {
        if (!GetComponent<PlayerController>()) {
            GameObject partical = Instantiate(particalPrefab, transform.position, Quaternion.identity);
            partical.GetComponent<SquarePartical>().StartPlay(GetComponent<SpriteRenderer>().sprite);
        }
    }
}

