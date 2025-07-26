using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SquareMoverToPlayerDecorator : SquareMoverDecorator
{
    PathFindManager pathFindManager;
    public List<E_CustomDir> moveTasks = new List<E_CustomDir>();
    Vector2Int currentNodePos;
    Vector2Int playerNodePos;

    //��ǰ·�����
    int currentIndex;
    public SquareMoverToPlayerDecorator(ISpecialPower power, SquareController _squareController, float _moveInterval = 0.8f, float _awakePrepareTime = 1.5f) : base(power, _squareController, _moveInterval, _awakePrepareTime){
    }

    protected override void FindPathMove()
    {
        base.FindPathMove();
        if (currentIndex < moveTasks.Count)
        {
            squareController.SquareMoveToTargetDir(moveTasks[currentIndex]);
            currentIndex++;
        }
        else
            UpdateTask();
    }
    public override void PowerInit()
    {
        base.PowerInit();
        pathFindManager = PathFindManager.Instance;
        //��ʼ���ȸ���·���б�
        UpdateTask();
    }

    public override void TriggerPower()
    {
        base.TriggerPower();
    }
    void UpdateTask()
    {
        if (pathFindManager == null)
            pathFindManager = PathFindManager.Instance;

        currentNodePos = pathFindManager.GetAStarNodeIndex(squareController);
        playerNodePos = pathFindManager.GetPlayerNodeIndex();
        moveTasks = pathFindManager.GetPathFindCommands(currentNodePos, playerNodePos);
        currentIndex = 0;
    }

    public override void PowerOnUpdate()
    {
        base.PowerOnUpdate();
        Debug.Log("Ѳ��׷���У�");
    }

}
