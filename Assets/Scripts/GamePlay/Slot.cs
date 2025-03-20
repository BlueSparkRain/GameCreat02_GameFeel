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

    private void Update()
    {
        if (transform.childCount == 0)
            isFull=false;
        //ʱ�̼�Ȿ���·��Ƿ��п�λ���п�λ���ɵ������ڷ���
        if (isFull && transform.GetSiblingIndex()<=6)
        {
            if (!transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<PlayerController>() && !transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Slot>().isFull)
            {
                StartCoroutine(ThrowSquare());
            }
        }
    }

    /// <summary>
    /// ���з��������������ɷ���
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.childCount > 1)
            StartCoroutine(transform.GetChild(0).GetComponent<Square>().LooseSelf());


        if (transform.parent.GetComponent<SquareColumn>().ColFull && !other.GetComponent<Square>().HasFather && !isFull )
        {
            if (transform.GetSiblingIndex() + 1 <= 7 && !transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Slot>().isFull && (transform.GetSiblingIndex() - 1) >= 0 && transform.parent.GetChild(transform.GetSiblingIndex() - 1).GetComponent<Slot>().isFull)
            {
                Debug.Log("��������");
                other.transform.SetParent(transform);
                currentSquare = other.GetComponent<Square>();

                if (!isFull && currentSquare != null && other.GetComponent<Square>())
                    other.GetComponent<Square>().MoveToSlot(transform.position);
                isFull = false;
                transform.parent.GetComponentInParent<SquareColumn>().UpdateTopSlot(transform.GetSiblingIndex());
                StartCoroutine(ThrowSquare());
            }

        }

        if (isFull)
            return;


        if (!isFull && !other.GetComponent<Square>().HasFather) 
        {
            other.transform.SetParent(transform);
            currentSquare = other.GetComponent<Square>();

            if (!isFull &&currentSquare != null && other.GetComponent<Square>())
               other.GetComponent<Square>().MoveToSlot(transform.position);
            isFull = true;
            transform.parent.GetComponentInParent<SquareColumn>().UpdateTopSlot(transform.GetSiblingIndex());
        }
    }
  
    /// <summary>
    /// �׳���ǰ���ڷ���
    /// </summary>
    public IEnumerator ThrowSquare()
    {
        if (currentSquare && currentSquare.GetComponent<PlayerController>() && currentSquare.GetComponent<PlayerController>().isSwaping)
            yield break;

        currentSquare = null;
        isFull = false;
        transform.parent.GetComponent<SquareColumn>().LooseOneSquare();
        yield return null;
        if (transform.childCount != 0)
            yield return transform?.GetChild(0).GetComponent<Square>().LooseSelf();

    }
}
