using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÿ����ֻ����һ���ؿ�
/// </summary>
public class Slot : MonoBehaviour
{
    public bool isFull;
    public Square currentSquare;

    public SquareColumn  selfColumn;

    Slot upslot;
    Slot downslot;


    public  bool isDownEmpty;

    private void Awake()
    {
        if(transform.childCount!=0)
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

    private void Update()
    {

        if (transform.childCount == 0)
            isFull=false;

        if (transform.childCount == 1)
            isFull = true;

        //��Ȿ���·��Ƿ��п�λ���п�λ���ɵ������ڷ���
        if (canCheckSelf && isFull && transform.GetSiblingIndex() <= 6)
        {
            if (!selfColumn.transform.GetChild(transform.GetSiblingIndex() + 1).GetComponent<PlayerController>()
                && !selfColumn.transform.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Slot>().isFull)
            //if (!selfColumn.transform.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Slot>().isFull)
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

        if (other.GetComponent<PlayerController>() && other.GetComponent<PlayerController>().isSwaping)
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
               other.GetComponent<Square>().MoveToSlot(transform.position);
            
            isFull = true;

            if (upslot)
                upslot.isDownEmpty = false;

            selfColumn.UpdateTopSlot(transform.GetSiblingIndex());
        }
    }
  
    /// <summary>
    /// �׳���ǰ���ڷ���
    /// </summary>
    public void ThrowSquare()
    {
        if (currentSquare && currentSquare.GetComponent<PlayerController>() && currentSquare.GetComponent<PlayerController>().isSwaping)
            return;

        //���۵��䣬���ݶ����ϲ�
        if (canCheckSelf && isFull && upslot)
        {
            if (!upslot.GetComponentInChildren<PlayerController>() && 
                 upslot.GetComponent<Slot>().isFull)
            {
                StartCoroutine(upslot.WaitLoose());
            }
        }

        //�ɵ��ϲ�
        if(upslot)
            upslot.isDownEmpty = true;

        currentSquare = null;
        selfColumn.LooseOneSquare();
        if (transform.childCount != 0)
           transform?.GetChild(0).GetComponent<Square>().LooseSelf();

    }

    IEnumerator WaitLoose()
    {
        canCheckSelf = false;
        yield return new WaitForSeconds(0.02f);
        canCheckSelf = true;
    }
}
