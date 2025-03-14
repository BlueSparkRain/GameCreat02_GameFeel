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

    private void Awake()
    {
        if(transform.childCount!=0)
            isFull = true;
    }

    /// <summary>
    /// ���з��������������ɷ���
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
    /// �׳����ڷ���
    /// </summary>
    public void ThrowSquare()
    {
        currentSquare = null;
        isFull = false;
        transform.parent.GetComponent<SquareColumn>().SquareThrow(transform.GetSiblingIndex());
    }
}
