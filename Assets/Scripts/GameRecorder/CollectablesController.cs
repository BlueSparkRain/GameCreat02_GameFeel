using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesController : MonoBehaviour
{
    [Header("收集物提示板")]
    public Transform targetCollectableContainer;

    //收集物提示栏UI预制件"
    GameObject UIObjPrefab;

    [Header("过关收集物目标配置")]
    public List<PlayerCollectablesMode> collectableTargetList;

    Dictionary<E_Collectable, PlayerCollectablesMode> collectDic = new Dictionary<E_Collectable, PlayerCollectablesMode>();

    void Start()
    {
        if (targetCollectableContainer != null)
        {

            UIObjPrefab = Resources.Load<GameObject>("Prefab/UIPanel/UIElement/TargetColletableObj");

            for (int i = 0; i < collectableTargetList.Count; i++)
            {
                if (!collectDic.ContainsKey(collectableTargetList[i].targetCollectableType))
                collectDic.Add(collectableTargetList[i].targetCollectableType, collectableTargetList[i]);
            }
     
            foreach (var item in collectDic)
            {
                item.Value.targetCollectableObj = Instantiate(UIObjPrefab, targetCollectableContainer).GetComponent<PlayerCollectablesViewer>();
                item.Value.targetCollectableObj.InitSelf(item.Value.targetNum, item.Value.targetCollectableSprite);
            }

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

    bool canAddStar = true;

    /// <summary>
    /// 收集任务达成检测
    /// </summary>
    void PassCheck()
    {
        for (int i = 0; i < collectableTargetList.Count; i++)
        {
            if (!collectableTargetList[i].reachTarget)
                return;
        }
        Pass();
    }

    void Pass()
    {
        if (canAddStar)
        {
            FindAnyObjectByType<ScoreController>().GetCollectData();
        }
        canAddStar = false;
        //Debug.Log("达成收集条件");
    }
}
