using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToPlayerMovePower : MovePower
{
    PathFindManager pathFindManager;
    //重新计算路线计时器
    //float updatePathTimer;
    //float updatePathInterval = 6;//4s更新一次

    public List<E_CustomDir> moveTasks = new List<E_CustomDir>();
    Vector2Int currentNodePos;
    Vector2Int playerNodePos;

    //当前路径序号
    int currentIndex;
    public ToPlayerMovePower(SquareController _squareController,  float _moveInterval = 1, float _awakePrepareTime = 2) : base(_squareController,  _moveInterval, _awakePrepareTime)
    {


    }

    public override void PowerInit()
    {
        base.PowerInit();
        pathFindManager = PathFindManager.Instance;
        
        //初始化先更新路径列表
        UpdateTask();
        //Debug.Log("找到路了！");
    }

    void UpdateTask()
    {
        //moveTasks.Clear();
        currentNodePos = pathFindManager.GetAStarNodeIndex(squareController);
        playerNodePos = pathFindManager.GetPlayerNodeIndex();
        //Debug.Log("P:" + playerNodePos + " C:" + currentNodePos);
        moveTasks = pathFindManager.GetPathFindCommands(currentNodePos, playerNodePos);
        currentIndex = 0;
    }

    public override void PowerOnUpdate()
    {
        base.PowerOnUpdate();
      
    }

    public override void TriggerPower()
    {
        base.TriggerPower();
    }

    protected override void FindPathMove()
    {
        base.FindPathMove();
        if (currentIndex < moveTasks.Count)
        {
            //Debug.Log("动了一下，是第" + currentIndex + "步");
            squareController.SquareMoveToTargetDir(moveTasks[currentIndex]);
            currentIndex++;
        }
        else
            UpdateTask();   
    } 
}
