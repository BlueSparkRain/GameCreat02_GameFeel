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

    /// <summary>
    /// 当有方块进入槽区域，容纳方块
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFull && !other.GetComponent<Square>().HasFather) 
        {
            other.transform.SetParent(transform);
            currentSquare = other.GetComponent<Square>();
            currentSquare.MoveToSlot(transform.position);
            isFull = true; 
            transform.parent.GetComponent<SquareColumn>().ActiveNewSlot();
        }
    }

    public void LooseSelf() 
    {
        currentSquare = null;
        isFull = false;
        transform.GetChild(0).GetComponent<Square>().LooseSelf();
    }

    /// <summary>
    /// 抛出槽内方块
    /// </summary>
    public void ThrowSquare()
    {
        currentSquare = null;
        isFull = false;
        transform.parent.GetComponent<SquareColumn>().SquareThrow(transform.GetSiblingIndex());
    }
}
