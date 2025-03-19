using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Square : MonoBehaviour,ICanEffect
{
    Rigidbody2D rb;
    [HideInInspector]
    public bool HasFather;
    [Header("�����÷�")]
    public int  BaseScore=50;

    [Header("���ؿ�����ƶ�")]
    public bool canMove=true;

    private GameObject particalPrefab => Resources.Load<GameObject>("Prefab/SquarePartical");


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (transform.parent && transform.parent.GetComponent<Slot>())
        {
            MoveToSlot(transform.parent.position);

            transform.parent.parent.parent.GetChild(transform.parent.GetSiblingIndex()).GetComponent<SquareRow>().SetRowSquare(this,transform.parent.parent.GetSiblingIndex());
        }

    }

    /// <summary>
    /// ʹ�������ƶ���Ŀ���λ��
    /// </summary>
    /// <param name="slotPos"></param>
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
            StartCoroutine(TweenHelper.MakeLerp(transform.position, truePos, 0.15f, val => transform.position = val));
        }
    }
    /// <summary>
    /// �ɵ�������
    /// </summary>
    public virtual IEnumerator LooseSelf()
    {
        transform.SetParent(null);
        yield return null;
        rb.velocity = new Vector3(0, -80);
        HasFather = false;
    }

    public virtual IEnumerator AnimScaleReMove()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.0f, 1.0f, 1.6f), 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
    }

    public virtual IEnumerator BeRemoved()
    {
        yield return null;
        Debug.Log("���鱻����");
        PitchTest.Instance.DingDong();

    }

    /// <summary>
    /// ִ�з��鼼���߼����ӷ�or������
    /// </summary>
    public virtual void DoSelfEffect()
    {
        Debug.Log("����Ч������");
        //�ӷ�
        FindAnyObjectByType<ScoreRecorder>().UpdatePlayerScore(BaseScore);
        ExplodeEffect();
    }

    /// <summary>
    /// ��������ȥ��ʱ�ı�ըЧ��
    /// </summary>
    protected virtual void ExplodeEffect() 
    {
        if (!GetComponent<PlayerController>()) {
            GameObject partical = Instantiate(particalPrefab, transform.position, Quaternion.identity);
            partical.GetComponent<SquarePartical>().StartPlay(GetComponent<SpriteRenderer>().sprite);
        }
      
    }
}

