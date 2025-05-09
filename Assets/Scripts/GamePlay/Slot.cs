using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 每个槽只接受一个地块
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
        
        //检测本槽下方是否有空位，有空位则松掉本槽内方块
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
    /// 当有方块进入槽区域，容纳方块
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
    /// 抛出当前槽内方块
    /// </summary>
    public void ThrowSquare()
    {
        ////本槽掉落，短暂冻结上槽
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

        //松掉上槽
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
