using System;
using UnityEngine;

[Serializable]
public class PlayerCollectablesMode
{
    [Header("Ŀ���ռ��ﾫ��")]
    public Sprite targetCollectableSprite;

    [Header("Ŀ���ռ�������")]
    public E_Collectable targetCollectableType;

    [Header("Ŀ���ռ�����")]
    public int targetNum;

    int currentNum;

    [Header("�Ѵ��")]
    public bool reachTarget;

    public PlayerCollectablesViewer targetCollectableObj;

    public void GetNewOneCollectable()
    {
        currentNum++;

        targetCollectableObj.UpdateSelf(currentNum);

        if (!reachTarget && currentNum >= targetNum)
            reachTarget = true;
    }
}

public enum E_Collectable
{
    ������,������,������
}
