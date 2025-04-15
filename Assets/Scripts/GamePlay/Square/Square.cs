using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Square : MonoBehaviour, ICanEffect
{
    Rigidbody2D rb;
    [HideInInspector]
    public bool HasFather;
    [Header("�����÷�")]
    public int BaseScore = 50;

    [Header("���ؿ�����ƶ�")]
    public bool canMove = true;
    protected GameObject particalPrefab => Resources.Load<GameObject>("Prefab/SquarePartical");

    protected Slot slot;

    protected virtual  void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Start()
    {
        StartCoroutine(HaveReady());
    }
    IEnumerator HaveReady()
    {
        yield return null;
        if (transform.parent && transform.parent.GetComponent<Slot>())
        {
            MoveToSlot(transform.parent.position);
            slot.transform.parent.parent.GetChild(transform.parent.GetSiblingIndex()).GetComponent<SquareRow>().SetRowSquare(this, slot.transform.parent.GetSiblingIndex());
        }
    }


    /// <summary>
    /// ʹ�������ƶ���Ŀ���λ��
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

        Vector3 truePos = new Vector3(slotPos.x, slotPos.y, -0.1f);
        if (transform.parent != null && transform.parent.GetComponent<Slot>() != null)
        {
            if (transform.parent.GetComponent<Slot>())
            {
                slot = transform.parent.GetComponent<Slot>();
                slot.selfColumn.UpdateColumnSquares(this, transform.parent.GetSiblingIndex());
                FindAnyObjectByType<SquareGroup>().UpdateRowSquares(transform.GetComponentInChildren<Square>(), slot.transform.parent.GetSiblingIndex(), slot.transform.GetSiblingIndex());
            }
            StartCoroutine(TweenHelper.MakeLerp(transform.position, truePos, 0.15f, val => transform.position = val));
        }
    }
    /// <summary>
    /// �ɵ�������
    /// </summary>
    public virtual void LooseSelf()
    {
        if (GetComponent<PlayerController>() != null && GetComponent<PlayerController>().isSwaping)
            return;

        transform.SetParent(null);
        HasFather = false;
        rb.velocity = new Vector3(0, -60);
    }

    /// <summary>
    /// ɫ����ֵĶ���
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator SquareBornAnim()
    {

        yield return null;
    }


    /// <summary>
    /// ɫ���ڽ���ʱ�Ķ���
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator SquareMoveAnim()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.0f, 1.0f, 1.6f), 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
    }

    /// <summary>
    /// ɫ���ڱ�����ʱ�Ķ���
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator SquareReMoveAnim()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.5f, 0.7f, 1.6f), 0.1f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(0.8f, 2.5f, 1.6f), 0.06f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 2.6f, 0.03f, val => transform.localScale = val);
    }

    public virtual IEnumerator BeRemoved()
    {
        yield return null;
        Debug.Log("���鱻����");
        FindAnyObjectByType<PitchTest>().DingDong();

    }

    /// <summary>
    /// ִ�з��鼼���߼����ӷ�or������
    /// </summary>
    public virtual void DoSelfEffect()
    {
        Debug.Log("����Ч������");
        //�ӷ�
        EventCenter.Instance.EventTrigger(E_EventType.E_GetSquareScore, BaseScore);
        ExplodeEffect();
    }

    /// <summary>
    /// ��������ȥ��ʱ�ı�ըЧ��
    /// </summary>
    protected virtual void ExplodeEffect()
    {
        if (!GetComponent<PlayerController>())
        {
            GameObject partical = Instantiate(particalPrefab, transform.position, Quaternion.identity);
            partical.GetComponent<SquarePartical>().StartPlay(GetComponent<SpriteRenderer>().sprite);
        }
    }
}

