using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CollectablesRecorder : MonoBehaviour
{
    [Header("收集物提示板")]
    public Transform targetCollectableContainer;

     //收集物提示栏UI预制件"
     GameObject UIObjPrefab;

    [Header("过关收集物目标配置")]
    public List<CollectablesUnit> collectableTargetList;

    Dictionary<E_Collectable, CollectablesUnit> collectDic=new Dictionary<E_Collectable, CollectablesUnit>();

    private void Awake()
    {
        UIObjPrefab = Resources.Load<GameObject>("Prefab/UIPanel/UIElement/TargetColletableObj");

        for (int i = 0; i < collectableTargetList.Count; i++)
        {
            if(!collectDic.ContainsKey(collectableTargetList[i].targetCollectableType))
            collectDic.Add(collectableTargetList[i].targetCollectableType, collectableTargetList[i]);
        }
    }

    void Start()
    {
        foreach (var item in collectDic)
        {
            item.Value.targetCollectableObj = Instantiate(UIObjPrefab,targetCollectableContainer).GetComponent<TargetCollectableObj>();
            item.Value.targetCollectableObj.InitSelf(item.Value.targetNum, item.Value.targetCollectableSprite);
        }
    }

    /// <summary>
    /// 获得一个收集物
    /// </summary>
    /// <param name="targetCollectableType"></param>
    public void GetCollectable(E_Collectable targetCollectableType) 
    {
        if (collectDic.ContainsKey(targetCollectableType))
        {
            collectDic[targetCollectableType].GetNewOneCollectable();
        }
        PassCheck();
    }


    /// <summary>
    /// 收集任务达成检测
    /// </summary>
    void PassCheck() 
    {
        for (int i = 0; i < collectableTargetList.Count; i++) 
        {
            if (!collectableTargetList[i].reachTarget)
            return;
            Pass();
        }
    }

    void Pass() 
    {
        Debug.Log("达成收集条件");
    }

}

[Serializable]
public class CollectablesUnit 
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

    public TargetCollectableObj targetCollectableObj;

    public void GetNewOneCollectable() 
    {
        currentNum++;

        targetCollectableObj.UpdateSelf(currentNum);

        if(!reachTarget && currentNum>=targetNum)
            reachTarget = true;
    }
}

public enum E_Collectable 
{ 
贝壳,章鱼,金币,

}
