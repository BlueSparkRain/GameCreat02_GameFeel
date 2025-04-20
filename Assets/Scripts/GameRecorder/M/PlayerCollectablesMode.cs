using System;
using UnityEngine;

[Serializable]
public class PlayerCollectablesMode
{
    [Header("目标收集物精灵")]
    public Sprite targetCollectableSprite;

    [Header("目标收集物类型")]
    public E_Collectable targetCollectableType;

    [Header("目标收集个数")]
    public int targetNum;

    int currentNum;

    [Header("已达成")]
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
    紫音符,红音符,蓝音符
}
