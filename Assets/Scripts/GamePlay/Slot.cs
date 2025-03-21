using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 每个槽只接受一个地块
/// </summary>
public class Slot : MonoBehaviour
{
    public bool isFull;
    public Square currentSquare;

    private void Awake()
    {
        if(transform.childCount!=0)
            isFull = true;
    }

    bool canCheckSelf = true;

    //public IEnumerator LooseUpSlotCheck() 
    //{
    //    if (!canCheckSelf)
    //        yield break;
    //    ThrowSquare();
    //}

    private void Update()
    {

        //Debug.Log(transform.childCount);

        if (transform.childCount == 0)
            isFull=false;

        if (transform.childCount == 1)
            isFull = true;

        //检测本槽下方是否有空位，有空位则松掉本槽内方块
        //if (transform.parent.GetComponent<SquareColumn>().ColFull && canCheckSelf && isFull && transform.GetSiblingIndex() <= 6)
        if ( canCheckSelf && isFull && transform.GetSiblingIndex() <= 6)
        {
            if (!transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<PlayerController>() && !transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Slot>().isFull)
            {
                //StartCoroutine(transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Slot>().WaitLoose());
                ThrowSquare();
            }
        }

    }

    /// <summary>
    /// 当有方块进入槽区域，容纳方块
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<PlayerController>() && other.GetComponent<PlayerController>().isSwaping)
            return;

        if (!transform.parent.GetComponent<SquareColumn>().isRemoving)
        {
            if (transform.parent.GetComponent<SquareColumn>().ColFull && !other.GetComponent<Square>().HasFather && !isFull)
            {
                if (transform.GetSiblingIndex() + 1 <= 7 && !transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Slot>().isFull && (transform.GetSiblingIndex() - 1) >= 0 && !transform.parent.GetChild(transform.GetSiblingIndex() - 1).GetComponent<Slot>().isFull)
                {

                    return;
                }
                else
                {
                    if (!isFull && transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Slot>() && !transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Slot>().isFull)
                    {
                        return;
                    }
                }
            }
        }

        //上下都有
        //if (!isFull && transform.GetSiblingIndex() + 1 <= 7 && transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Slot>().isFull && (transform.GetSiblingIndex() - 1) >= 0 && transform.parent.GetChild(transform.GetSiblingIndex() - 1).GetComponent<Slot>().isFull)
        //{
        //    return;
        //}



        if (isFull)
        return;

        if (!isFull && !other.GetComponent<Square>().HasFather) 
        {
            other.transform.SetParent(transform);
            currentSquare = other.GetComponent<Square>();


            StartCoroutine(WaitLoose());


            if (!isFull &&currentSquare != null && other.GetComponent<Square>())
               other.GetComponent<Square>().MoveToSlot(transform.position);
            isFull = true;

            transform.parent.GetComponentInParent<SquareColumn>().UpdateTopSlot(transform.GetSiblingIndex());
        }
    }
  
    /// <summary>
    /// 抛出当前槽内方块
    /// </summary>
    public void ThrowSquare()
    {
        //if (!canCheckSelf)
        //    return;

        if (currentSquare && currentSquare.GetComponent<PlayerController>() && currentSquare.GetComponent<PlayerController>().isSwaping)
            return;

        //本槽掉落，短暂冻结上槽
        if (canCheckSelf && isFull && transform.GetSiblingIndex() > 0)
        {
            if (!transform.parent.GetChild(transform.GetSiblingIndex() - 1).GetComponent<PlayerController>() &&  transform.parent.GetChild(transform.GetSiblingIndex() - 1).GetComponent<Slot>().isFull)
            {
                StartCoroutine(transform.parent.GetChild(transform.GetSiblingIndex() - 1).GetComponent<Slot>().WaitLoose());
            }
        }

        //StartCoroutine(WaitLoose());

        if (currentSquare && currentSquare.GetComponent<PlayerController>() &&  currentSquare.GetComponent<PlayerController>().isSwaping)
            //yield break;
            return;

        currentSquare = null;
        //isFull = false;
        transform.parent.GetComponent<SquareColumn>().LooseOneSquare();
        //yield return null;
        if ( transform.childCount != 0)
           transform?.GetChild(0).GetComponent<Square>().LooseSelf();

    }

    IEnumerator WaitLoose()
    {
        canCheckSelf = false;
        yield return new WaitForSeconds(0.01f);
        canCheckSelf = true;
    }
}
