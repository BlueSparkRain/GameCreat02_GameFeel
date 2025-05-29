using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesController : MonoBehaviour
{
    [Header("�ռ�����ʾ��")]
    public Transform targetCollectableContainer;

    //�ռ�����ʾ��UIԤ�Ƽ�"
    GameObject UIObjPrefab;

    [Header("�����ռ���Ŀ������")]
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
    /// ���һ���ռ���
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
    /// �ռ������ɼ��
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
        //Debug.Log("����ռ�����");
    }
}
