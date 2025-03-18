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


    private void Update()
    {
        if (transform.childCount == 0)
            isFull=false;

        //if (!isFull && transform.parent.GetComponent<SquareColumn>().FirstEmptySlotIndex!=0)
        //{
        //   if(transform.GetSiblingIndex() - 1 >=0 && transform.parent.GetChild(transform.GetSiblingIndex() - 1).childCount!=0)
        //    StartCoroutine(transform.parent.GetChild(transform.GetSiblingIndex()-1).GetComponent<Slot>().ThrowSquare());
        //}


        //时刻检测本槽下方是否有空位，有空位则松掉本槽内方块
        if (isFull &&  transform.GetSiblingIndex()<=6)
        {
            if (!transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<PlayerController>()&&!transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Slot>().isFull)
                StartCoroutine(ThrowSquare());
        }
    }

    /// <summary>
    /// 当有方块进入槽区域，容纳方块
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFull)
            return;

        if (!isFull && !other.GetComponent<Square>().HasFather) 
        {
            other.transform.SetParent(transform);
            currentSquare = other.GetComponent<Square>();

            if (!isFull &&currentSquare != null && other.GetComponent<Square>())
            {
               //StartCoroutine(other.GetComponent<Square>().MoveToSlot(transform.position));
               other.GetComponent<Square>().MoveToSlot(transform.position);
            }
            isFull = true;

            transform.parent.GetComponentInParent<SquareColumn>().UpdateTopSlot(transform.GetSiblingIndex());
        }
    }
  
    /// <summary>
    /// 抛出槽内方块
    /// </summary>
    public IEnumerator ThrowSquare()
    {
        currentSquare = null;
        isFull = false;
        transform.parent.GetComponent<SquareColumn>().LooseOneSquare();
        yield return null;
        if (transform.childCount != 0)
            yield return transform?.GetChild(0).GetComponent<Square>().LooseSelf();
    }
}
