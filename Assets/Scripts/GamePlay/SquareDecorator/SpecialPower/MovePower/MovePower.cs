using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_MoveType
{
    ���, ���, ����
}


public class MovePower : IHaveSpecialPower
{
    protected bool canMove;
    protected float moveTimer;
    protected float moveInterval;
    //protected E_MoveType moveType;
    protected WaitForSeconds awakeDealy;
    protected SquareController squareController;

    public MovePower(SquareController _squareController, float _moveInterval = 1, float _awakePrepareTime = 2)
    {
        squareController = _squareController;
        //moveType = _moveType;
        moveInterval = _moveInterval;
        awakeDealy = new WaitForSeconds(_awakePrepareTime);
        canMove = false;
    }
    protected bool nextMove;

    public virtual void PowerInit()
    {
        canMove = true;
    }
    public virtual void PowerOnUpdate()
    {
        if (canMove)
        {

            if (moveTimer >= 0)
                moveTimer -= Time.deltaTime;
            else
                nextMove = true;

            if (nextMove)
            {
                nextMove = false;
                moveTimer = moveInterval;

                if(!squareController.isSwaping)
                FindPathMove();
                

                //������һ�����ж�·�ߣ�ִ���ж�����
            }
        }
    }

    protected virtual void FindPathMove()
    {

    }

    /// <summary>
    /// ׷��ǰԤ��
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual void TriggerPower()
    {
        MonoManager.Instance.StartCoroutine(AwakeFromSleep());
    }

    IEnumerator AwakeFromSleep()
    {
        yield return awakeDealy;
        canMove = true;
    }

}
