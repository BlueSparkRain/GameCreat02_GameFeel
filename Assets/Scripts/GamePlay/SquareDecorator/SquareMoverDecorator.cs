using System.Collections;
using UnityEngine;

public enum E_MoveType
{
    ���, ���, ����
}

public abstract class SquareMoverDecorator : SquareDecorator
{
    protected bool Iawake;
    protected float moveTimer;
    protected float moveInterval;
    protected WaitForSeconds awakeDealy;
    protected SquareController squareController;

    protected bool nextMove;

    public SquareMoverDecorator(ISpecialPower power, SquareController _squareController, float _moveInterval = 1, float _awakePrepareTime = 2) : base(power)
    {
        squareController = _squareController;
        moveInterval = _moveInterval;
        awakeDealy = new WaitForSeconds(_awakePrepareTime);
        Iawake = false;
    }

    public override void PowerInit()
    {
        base.PowerInit();
        MonoManager.Instance.StartCoroutine(AwakeFromSleep());


    }

    public override void PowerOnUpdate()
    {
        base.PowerOnUpdate();

        if (Iawake)
        {

            if (moveTimer >= 0)
                moveTimer -= Time.deltaTime;
            else
                nextMove = true;

            if (nextMove)
            {
                nextMove = false;
                moveTimer = moveInterval;

                if (!squareController.isSwaping)
                    FindPathMove();


                //������һ�����ж�·�ߣ�ִ���ж�����
            }
        }
    }
    public override void TriggerPower()
    {
        base.TriggerPower();
    
    }

    protected virtual void FindPathMove()
    {

    }


    IEnumerator AwakeFromSleep()
    {
        yield return awakeDealy;
        Iawake = true;
        Debug.Log("������");
    }

}
