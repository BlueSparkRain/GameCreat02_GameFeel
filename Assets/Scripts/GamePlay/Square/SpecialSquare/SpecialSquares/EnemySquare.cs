using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ˣ����⣩����
/// </summary>
public class EnemySquare : ColorSquare
{
    //public AnimationCurve moveCurve;

    //[Header("ִ�в�����Ŀ�귽��")]
    //public GameObject targetSquare;

    //public LayerMask whatIsSquare;
    //float timer = 0;

    //public bool canAct;

    //[Header("�ж����")]
    //public float actInterval = 0.25f;

    //[Header("������")]
    //public float targetSquareChecckDistance = 1.2f;

    //public bool isSwaping = false;

    //bool canswap = true;

    //float interval = 0.2f;

    //bool sleep;

    //public override IEnumerator BeRemoved()
    //{
    //    yield  return base.BeRemoved();
    //    //���˱�����

    //}

    ////�������·���Ϸ���
    //private List<Square> pathToPlayer = new List<Square>();

    ///// <summary>
    ///// �õ�·��
    ///// </summary>
    //void GetPathSquaresToPlayer() 
    //{
    ////�����ʷ·��
    //pathToPlayer.Clear();
    

    //}

    ///// <summary>
    ///// ��õ�ǰ·������һ������
    ///// </summary>
    ///// <returns></returns>
    //Square GetTargetSquare() {

    //    return null;
    //}

    ///// <summary>
    ///// ��Ŀ�귽�黥��λ��
    ///// </summary>
    ///// <param name="otherSquare"></param>
    ///// <returns></returns>
    //public IEnumerator Swap(Square otherSquare)
    //{
    //    if (!canswap || !otherSquare.transform.parent || otherSquare.transform.parent.childCount > 1 || !otherSquare.transform.parent.GetComponent<WalkableSlot>().isFull || !otherSquare.GetComponent<Square>().canMove)
    //    {
    //        Debug.Log("�޷�������");
    //        yield break;
    //    }

    //    if (transform.parent == null || !transform.parent.GetComponent<WalkableSlot>().isFull)
    //        yield break;
    //    if (!transform.parent || !otherSquare.transform.parent || !otherSquare.transform.parent.GetComponent<WalkableSlot>() || !transform.parent.GetComponent<WalkableSlot>())
    //        yield break;

    //    IsSwapingCheck();

    //    //������Ч����
    //    MusicManager.Instance.PlaySound("swap", 2);

    //    canswap = false;
    //    Transform mySlot = transform.parent;
    //    otherSquare.HasFather = false;
    //    HasFather = false;
    //    transform.SetParent(otherSquare.transform.parent);
    //    otherSquare.transform.SetParent(mySlot);

    //    if (transform.parent != null && transform.parent.GetComponent<WalkableSlot>())
    //        MoveToSlot(transform.parent.position);

    //    if (otherSquare != null && mySlot != null)
    //    {
    //        otherSquare.MoveToSlot(mySlot.position);
    //    }
    //    if (transform.parent != null)
    //    {
    //        StartCoroutine(GoMove(transform.position, transform.parent.position, 0.1f));
    //        if (transform.parent.GetComponent<WalkableSlot>())
    //        {
    //            WalkableSlot slot = transform.parent.GetComponent<WalkableSlot>();
    //            slot.transform.parent.GetComponent<SquareColumn>().UpdateColumnSquares(this, slot.transform.GetSiblingIndex());
    //            FindAnyObjectByType<SquareGroup>().UpdateRowSquares(transform.GetComponentInChildren<Square>(), slot.transform.parent.GetSiblingIndex(), slot.transform.GetSiblingIndex());
    //        }
    //        canAct = false;
    //        timer = actInterval;
    //        canswap = true;
    //        yield return SquareMoveAnim();
    //    }
    //}

    //IEnumerator GoMove(Vector3 startPos, Vector3 targetPos, float duration)
    //{
    //    float timer = 0;
    //    while (timer <= duration)
    //    {
    //        timer += Time.unscaledDeltaTime;
    //        transform.position = Vector3.Lerp(startPos, targetPos, moveCurve.Evaluate(timer / duration));
    //        yield return null;
    //    }
    //}

    //float swapTimer;
    //float swapInterval = 0.3f;
    //void IsSwapingCheck()
    //{
    //    if (isSwaping)
    //    {
    //        swapTimer = swapInterval;
    //    }
    //    else
    //    {
    //        swapTimer = swapInterval;
    //        isSwaping = true;
    //    }
    //}
}
