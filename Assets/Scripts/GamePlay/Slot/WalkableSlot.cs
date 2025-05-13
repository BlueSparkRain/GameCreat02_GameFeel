using System.Collections;
using UnityEngine;

public enum E_SlotType
{
    walkableSlot,
    obstacleSlot,
    spawnerSlot,
}

/// <summary>
/// ÿ����ֻ����һ���ؿ�
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
        if (checkTimer > 0)
        {
            checkTimer -= Time.deltaTime;
            //canCheck=false;
        }
        else
            canCheck = true;

        //if(isFull && downslot) 
        //{
        //    if (!downslot.isFull) 
        //    {
        //        ThrowSquare();
        //    }
        //}

        //��Ȿ���·��Ƿ��п�λ���п�λ���ɵ������ڷ���
        if (isFull && canCheck && downslot)
        {
            checkTimer = checkInterval;
            canCheck = false;

            if (!downslot.isFull)
            {
                ThrowSquare();
                //if (transform.childCount != 0)
                //    transform?.GetChild(0).GetComponent<SquareController>().SquareLoose();
            }
        }
    }

    public void SetSquare(Square newSquare) 
    {
        if (isFull)
            return;
        currentSquare =newSquare;
        isFull =  true;
        StartCoroutine(WaitLoose());
        newSquare.GetComponent<SquareController>().SquareMoveToSlot(transform.position);

        //if (upslot)
        //    upslot.isDownEmpty = false;
    }

    /// <summary>
    /// ���з��������������ɷ���
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
    /// �׳���ǰ���ڷ���
    /// </summary>
    public void ThrowSquare(float interval=0.1f)
    {
        ////���۵��䣬���ݶ����ϲ�
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

            //if(downslot && downslot.isFull)
            //currentSquare.GetComponent<SquareController>().SquareLoose();
            currentSquare = null;
        }

        selfColumn.UpdateSubColumnSquares(null, transform.GetSiblingIndex());

        if (transform.childCount != 0)
            transform?.GetChild(0).GetComponent<SquareController>().SquareLoose();

    }
    public  IEnumerator WaitLoose(float interval=0.1f)
    {
        checkTimer = interval;
        canCheck = false;
       
        selfColumn.LooseASlot();
        yield return null;
    }
}
