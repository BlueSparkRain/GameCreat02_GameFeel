using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CollectablesRecorder : MonoBehaviour
{
    [Header("�ռ�����ʾ��")]
    public Transform targetCollectableContainer;

     //�ռ�����ʾ��UIԤ�Ƽ�"
     GameObject UIObjPrefab;

    [Header("�����ռ���Ŀ������")]
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


    /// <summary>
    /// �ռ������ɼ��
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
        Debug.Log("����ռ�����");
    }

}

[Serializable]
public class CollectablesUnit 
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
����,����,���,

}
