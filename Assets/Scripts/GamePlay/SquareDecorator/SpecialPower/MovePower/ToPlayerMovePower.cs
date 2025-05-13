using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToPlayerMovePower : MovePower
{
    PathFindManager pathFindManager;
    //���¼���·�߼�ʱ��
    //float updatePathTimer;
    //float updatePathInterval = 6;//4s����һ��

    public List<E_CustomDir> moveTasks = new List<E_CustomDir>();
    Vector2Int currentNodePos;
    Vector2Int playerNodePos;

    //��ǰ·�����
    int currentIndex;
    public ToPlayerMovePower(SquareController _squareController,  float _moveInterval = 1, float _awakePrepareTime = 2) : base(_squareController,  _moveInterval, _awakePrepareTime)
    {


    }

    public override void PowerInit()
    {
        base.PowerInit();
        pathFindManager = PathFindManager.Instance;
        
        //��ʼ���ȸ���·���б�
        UpdateTask();
        //Debug.Log("�ҵ�·�ˣ�");
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
            //Debug.Log("����һ�£��ǵ�" + currentIndex + "��");
            squareController.SquareMoveToTargetDir(moveTasks[currentIndex]);
            currentIndex++;
        }
        else
            UpdateTask();   
    } 
}
