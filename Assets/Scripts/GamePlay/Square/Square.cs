using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Square : MonoBehaviour,ICanEffect
{
    Rigidbody2D rb;
    [HideInInspector]
    public bool HasFather;
    [Header("基础得分")]
    public int  BaseScore=50;

    private GameObject particalPrefab => Resources.Load<GameObject>("Prefab/SquarePartical");

    public bool canAct = true;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

 

    protected virtual void Start()
    {
    }
    public virtual void MoveToSlot(Vector3 slotPos)
    {
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
            //yield return TweenHelper.MakeLerp(transform.position, truePos, 0.15f, val => transform.position = val);
            StartCoroutine(TweenHelper.MakeLerp(transform.position, truePos, 0.15f, val => transform.position = val));
        }
    }
    /// <summary>
    /// 松掉本方块
    /// </summary>
    public IEnumerator LooseSelf()
    {
        transform.SetParent(null);
        yield return null;
        rb.velocity = new Vector3(0, -80);
        HasFather = false;
    }

    public virtual IEnumerator BeRemoved()
    {
        yield return null;
        DoSelfExcute();
    }

    public virtual void DoSelfExcute()
    {
        //加分
        FindAnyObjectByType<ScoreRecorder>().UpdatePlayerScore(BaseScore);
        ExplodeEffect();
    }

    /// <summary>
    /// 自身被去除时的爆炸效果
    /// </summary>
    protected virtual void ExplodeEffect() 
    {
        if (!GetComponent<PlayerController>()) {
            GameObject partical = Instantiate(particalPrefab, transform.position, Quaternion.identity);
            partical.GetComponent<SquarePartical>().StartPlay(GetComponent<SpriteRenderer>().sprite);
        }
      
    }
}

