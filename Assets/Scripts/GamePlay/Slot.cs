using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ÿ����ֻ����һ���ؿ�
/// </summary>
public class Slot : MonoBehaviour
{
    public bool isFull;
    public Square currentSquare;

    public SquareColumn selfColumn;

    Slot upslot;
    Slot downslot;


    public bool isDownEmpty;

    private void Awake()
    {
        if (transform.childCount != 0)
            isFull = true;
    }

    private void Start()
    {
        selfColumn = transform.parent.GetComponent<SquareColumn>();
        if (transform.GetSiblingIndex() != 0)
            upslot = selfColumn.columnSlots[transform.GetSiblingIndex() - 1].GetComponent<Slot>();

        if (transform.GetSiblingIndex() != 7)
            downslot = selfColumn.columnSlots[transform.GetSiblingIndex() + 1].GetComponent<Slot>();

    }
    bool canCheckSelf = true;

    float checkTimer;
    float checkInterval=0.15f;
    bool canCheck=true;

    private void Update()
    {
        if (checkTimer >= 0)
        {
         checkTimer -= Time.deltaTime;
        }
        else
        canCheck = true;
        
        //��Ȿ���·��Ƿ��п�λ���п�λ���ɵ������ڷ���
        if (canCheck && canCheckSelf && isFull && downslot)
        {
            canCheck=false;
            
            if (!downslot.isFull)
            {
                ThrowSquare();
            }
        }
    }

    /// <summary>
    /// ���з��������������ɷ���
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDownEmpty)
            return;

        if (isFull)
            return;

        if (!selfColumn.isRemoving)
        {
            if (selfColumn.ColFull && !other.GetComponent<Square>().HasFather && !isFull)
            {
                if (downslot && !downslot.isFull
                    && upslot && !upslot.isFull)
                    return;
                else
                {
                    if (!isFull &&
                        downslot && !downslot.isFull)
                        return;
                }
            }
        }

        if (!isFull && !other.GetComponent<Square>().HasFather)
        {
            other.transform.SetParent(transform);
            currentSquare = other.GetComponent<Square>();

            StartCoroutine(WaitLoose());

            if (!isFull && currentSquare != null && other.GetComponent<Square>())
            {
                other.GetComponent<Square>().MoveToSlot(transform.position);
            }

            isFull = true;

            if (upslot)
                upslot.isDownEmpty = false;

        }
    }

    /// <summary>
    /// �׳���ǰ���ڷ���
    /// </summary>
    public void ThrowSquare()
    {
        ////���۵��䣬���ݶ����ϲ�
        if (canCheckSelf && isFull && upslot)
        //if (isFull && upslot)
        {
            if (!upslot.GetComponentInChildren<PlayerController>() &&
                 upslot.GetComponent<Slot>().isFull)
            {
                StartCoroutine(upslot.WaitLoose());
            }
        }
        isFull = false;

        //�ɵ��ϲ�
        if (currentSquare && upslot)
        {
            upslot.isDownEmpty = true;
            currentSquare = null;
        }
        selfColumn.UpdateColumnSquares(null, transform.GetSiblingIndex());

        //square.LooseSelf();
        if (transform.childCount != 0)
            transform?.GetChild(0).GetComponent<Square>().LooseSelf();
    }
    IEnumerator WaitLoose()
    {
        checkTimer=checkInterval;
        canCheckSelf = false;
        yield return new WaitForSeconds(0.08f);
        canCheckSelf = true;
        selfColumn.LooseASlot();
    }
}
