using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人（特殊）方块
/// </summary>
public class EnemySquare : ColorSquare
{
    //public AnimationCurve moveCurve;

    //[Header("执行操作的目标方块")]
    //public GameObject targetSquare;

    //public LayerMask whatIsSquare;
    //float timer = 0;

    //public bool canAct;

    //[Header("行动间隔")]
    //public float actInterval = 0.25f;

    //[Header("检测距离")]
    //public float targetSquareChecckDistance = 1.2f;

    //public bool isSwaping = false;

    //bool canswap = true;

    //float interval = 0.2f;

    //bool sleep;

    //public override IEnumerator BeRemoved()
    //{
    //    yield  return base.BeRemoved();
    //    //敌人被消除

    //}

    ////到达玩家路径上方块
    //private List<Square> pathToPlayer = new List<Square>();

    ///// <summary>
    ///// 得到路径
    ///// </summary>
    //void GetPathSquaresToPlayer() 
    //{
    ////清空历史路径
    //pathToPlayer.Clear();
    

    //}

    ///// <summary>
    ///// 获得当前路径上下一个方块
    ///// </summary>
    ///// <returns></returns>
    //Square GetTargetSquare() {

    //    return null;
    //}

    ///// <summary>
    ///// 与目标方块互换位置
    ///// </summary>
    ///// <param name="otherSquare"></param>
    ///// <returns></returns>
    //public IEnumerator Swap(Square otherSquare)
    //{
    //    if (!canswap || !otherSquare.transform.parent || otherSquare.transform.parent.childCount > 1 || !otherSquare.transform.parent.GetComponent<WalkableSlot>().isFull || !otherSquare.GetComponent<Square>().canMove)
    //    {
    //        Debug.Log("无法交换！");
    //        yield break;
    //    }

    //    if (transform.parent == null || !transform.parent.GetComponent<WalkableSlot>().isFull)
    //        yield break;
    //    if (!transform.parent || !otherSquare.transform.parent || !otherSquare.transform.parent.GetComponent<WalkableSlot>() || !transform.parent.GetComponent<WalkableSlot>())
    //        yield break;

    //    IsSwapingCheck();

    //    //交换音效播放
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
