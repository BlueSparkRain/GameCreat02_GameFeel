using System.Collections;
using UnityEngine;

public enum E_SlotType
{
    walkableSlot,
    obstacleSlot,
    spawnerSlot,
}

/// <summary>
/// 每个槽只接受一个地块
/// </summary>
public class WalkableSlot :Slot
{
    public bool isFull;
    public Square currentSquare;
    public WalkableSlot upslot;
    WalkableSlot downslot;
    public bool isDownEmpty=true;

    private void Awake()
    {
        if (transform.childCount != 0)
            isFull = true;
    }

    bool canCheckSelf = false;

    float checkTimer;
    float checkInterval = 0.1f;
    bool canCheck = true;

    bool isButtom;

    public void IsButtom()
    {
        isDownEmpty = false;
        isButtom = true;
    }

    public void InitUp_DownSlot(WalkableSlot upslot,WalkableSlot downslot)
    {
        this.upslot=upslot;
        this.downslot = downslot;
        isDownEmpty = true;
    }

    float canCheckSelfTimer;
    float canCheckSelfInterval;

    private void Update()
    {
        //检测本槽下方是否有空位，有空位则松掉本槽内方块
        if (isFull && canCheck && downslot)
        {
            checkTimer = checkInterval;
            canCheck = false;

            if (!downslot.isFull)
            {
                ThrowSquare();
            }
        }
    }

    public void SetSquare(Square newSquare) 
    {
        if (isFull)
            return;
        currentSquare =newSquare;
        newSquare.transform.SetParent(transform);
        isFull =  true;
        StartCoroutine(WaitLoose());
        newSquare.GetComponent<SquareController>().SquareMoveToSlot(transform.position);
    }

    /// <summary>
    /// 当有方块进入槽区域，容纳方块
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.childCount>1 || isFull || isDownEmpty)
            return;

        if (!selfColumn.isRemoving)
        {
            if (selfColumn.subColFull && !other.GetComponent<Square>().HasFather && !isFull)
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
                other.GetComponent<SquareController>().SquareMoveToSlot(transform.position);
                isFull = true;
            }

            if (upslot)
                upslot.isDownEmpty = false;
        }
    }


    /// <summary>
    /// 抛出当前槽内方块
    /// </summary>
    public void ThrowSquare(float interval=0.08f)
    {
        ////本槽掉落，短暂冻结上槽
        if (isFull && upslot)
        {
            if (upslot.GetComponent<WalkableSlot>().isFull)
            {
                StartCoroutine(upslot.WaitLoose(interval));
            }
        }
        isFull = false;

        if (currentSquare && upslot)
        {
            upslot.isDownEmpty = true;
            isDownEmpty = false;
            currentSquare = null;
        }

        selfColumn.UpdateSubColumnSquares(null, transform.GetSiblingIndex());

        if (transform.childCount != 0)
            transform?.GetChild(0).GetComponent<SquareController>().SquareLoose();

    }
    public  IEnumerator WaitLoose(float interval=0.08f)
    {
        //checkTimer = interval;
        canCheck = false;
        yield return new WaitForSeconds(interval);
        canCheck = true;
       
        selfColumn.LooseASlot();
        yield return null;
    }
}
