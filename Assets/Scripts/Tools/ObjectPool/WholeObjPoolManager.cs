using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WholeObjPoolManager :MonoSingleton<WholeObjPoolManager>
{
    #region ���ӳ� Info
    public List<int> particlPoolsCapicity = new List<int>(4) {5,5,5,15};
    [Header("�������ĳ�")]
    public List<GameObject> perfactPool = new List<GameObject>();
    [Header("������ĳ�")]
    public List<GameObject> nicePool = new List<GameObject>();
    [Header("�úý��ĳ�")]
    public List<GameObject> goodPool = new List<GameObject>();
    [Header("���ӱ�ը���ĳ�")]
    public List<GameObject> squareExplodePool = new List<GameObject>();

    private GameObject perfactPrefab;
    private GameObject nicePrefab;
    private GameObject goodPrefab;
    private GameObject squareExplodetPrefab;
    #endregion

    #region �������ĳ� Info
    [Header("����������")]
    public int chartPoolCapcity = 5;
    [Header("�������ĳ�")]
    public List<GameObject> chartPool = new List<GameObject>();
    [Header("����Ԥ�Ƽ�")]
    private GameObject chartPrefab;
    #endregion

    #region �����
    [Header("ɫ���")]
    public List<GameObject> colorSquarePool = new List<GameObject>();
    [Header("�ռ����")]
    public List<GameObject> collectableSquarePool = new List<GameObject>();
    [Header("�����������")]
    public List<GameObject> triggerRemoveSquarePool = new List<GameObject>();


    [Header("ɫ��Ԥ�Ƽ�")]
    private GameObject  colorSquarePrefab;
    [Header("�ռ���Ԥ�Ƽ�")]
    private GameObject collectableSquarePrefab;
    [Header("����������Ԥ�Ƽ�")]
    private GameObject triggerRemoveSquarePrefab;
    #endregion

    public Transform PerfactPool;
    public Transform NicePool;
    public Transform GoodPool;

    public Transform SquareExplodePool;
    
    public Transform ChartPool;

    public Transform ColorSquarePool;
    public Transform CollectableSquarePool;
    public Transform TriggerRemoveSquarePool;

    public void LoadNewPool() 
    {
        Debug.Log("�����µĳ���");

        perfactPool = new List<GameObject>();

        nicePool = new List<GameObject>();

        goodPool = new List<GameObject>();

        squareExplodePool = new List<GameObject>();


        chartPool = new List<GameObject>();

        colorSquarePool = new List<GameObject>();

        collectableSquarePool = new List<GameObject>();

        triggerRemoveSquarePool = new List<GameObject>();

        //�ҵ������е�GameMap,����������Ҫ�ĳ�
        FullWholePool(perfactPrefab, 5, perfactPool, PerfactPool);
        FullWholePool(nicePrefab, 5, nicePool, NicePool);
        FullWholePool(goodPrefab, 5, goodPool, GoodPool);
        FullWholePool(squareExplodetPrefab, 15, squareExplodePool, SquareExplodePool);


        FullWholePool(colorSquarePrefab, 90, colorSquarePool, ColorSquarePool);
        FullWholePool(collectableSquarePrefab, 10, collectableSquarePool, CollectableSquarePool);
        FullWholePool(triggerRemoveSquarePrefab, 20, triggerRemoveSquarePool, TriggerRemoveSquarePool);

        FullWholePool(chartPrefab, chartPoolCapcity, chartPool, ChartPool);
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener(E_EventType.E_CurrentLevelOver,ClearAllPool);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_CurrentLevelOver,ClearAllPool);
    }

    void ClearAllPool() 
    {
        ClearTargetPool(PerfactPool,perfactPool);
        ClearTargetPool(NicePool, nicePool);
        ClearTargetPool(GoodPool, goodPool);
        ClearTargetPool(SquareExplodePool, squareExplodePool);
        ClearTargetPool(ColorSquarePool, colorSquarePool);
        ClearTargetPool(ChartPool, chartPool);
        ClearTargetPool(CollectableSquarePool, collectableSquarePool);
        ClearTargetPool(TriggerRemoveSquarePool, triggerRemoveSquarePool);
    }

    void ClearTargetPool(Transform poolFather,List<GameObject> pool) 
    {
        pool.Clear();
        for (int i = 0; i < poolFather.childCount; i++)
        {
            Destroy(poolFather.GetChild(i).gameObject);
        }
    }


    protected override void InitSelf()
    {
        if (!hasInit)
        {
            hasInit =true;

            perfactPrefab = Resources.Load<GameObject>("Prefab/Partical/Player_Hit_Perfact");
            nicePrefab = Resources.Load<GameObject>("Prefab/Partical/Player_Hit_Nice");
            goodPrefab = Resources.Load<GameObject>("Prefab/Partical/Player_Hit_Good");
            squareExplodetPrefab = Resources.Load<GameObject>("Prefab/Partical/Square_ExplodeEffect");

            colorSquarePrefab = Resources.Load<GameObject>("Prefab/Square/ColorSquare");
            collectableSquarePrefab = Resources.Load<GameObject>("Prefab/Square/CollectableSquare");
            triggerRemoveSquarePrefab = Resources.Load<GameObject>("Prefab/Square/TriggerRemovableSquare");
            
            chartPrefab = Resources.Load<GameObject>("Prefab/Chart/Chart");
        }
    }
    
    /// <summary>
    /// ����ɫ��������
    /// </summary>
    /// <param name="squareType"></param>
    /// <returns></returns>
    public GameObject GetTargetSquareObj(E_SquareType squareType) 
    {
        switch (squareType)
        {
            case E_SquareType.ɫ��:

                return GetInstnceFromPool(colorSquarePrefab, colorSquarePool, ColorSquarePool); 
            case E_SquareType.�ռ���:
                return GetInstnceFromPool(collectableSquarePrefab, collectableSquarePool, CollectableSquarePool);
            case E_SquareType.����������:
                return GetInstnceFromPool(triggerRemoveSquarePrefab, triggerRemoveSquarePool, TriggerRemoveSquarePool); 

            default:
                return GetInstnceFromPool(colorSquarePrefab, colorSquarePool, ColorSquarePool); 
               
        }
    }


    public GameObject GetTargetParticalObj(E_ParticalType particalType) 
    {
        switch (particalType)
        {
            case E_ParticalType.��ҿ�������:
                return GetInstnceFromPool(perfactPrefab, perfactPool, PerfactPool); 
            case E_ParticalType.��ҿ�������:
                return GetInstnceFromPool(nicePrefab, nicePool, NicePool);
            case E_ParticalType.��ҿ����:
                return GetInstnceFromPool(goodPrefab, goodPool, GoodPool);
            case E_ParticalType.ɫ��������ը:
                return GetInstnceFromPool(squareExplodetPrefab, squareExplodePool, SquareExplodePool);
            default:
                return GetInstnceFromPool(squareExplodetPrefab, squareExplodePool, SquareExplodePool);
        }
    }

    public GameObject GetChartObj() 
    {
         return GetInstnceFromPool(chartPrefab,chartPool,ChartPool);
    }

    /// <summary>
    /// �����������
    /// </summary>
    public void ObjReturnPool(E_ObjectPoolType poolType,GameObject instance)
    {
        switch (poolType)
        {
            case E_ObjectPoolType.��ҿ���������:
                instance.transform.SetParent(PerfactPool);
                break;
            case E_ObjectPoolType.��ҿ��������:
                instance.transform.SetParent(NicePool);
                break;
            case E_ObjectPoolType.��ҿ���ó�:
                instance.transform.SetParent(GoodPool);
                break;
            case E_ObjectPoolType.ɫ��������ը��:
                instance.transform.SetParent(SquareExplodePool);
                break;
            case E_ObjectPoolType.������:
                instance.transform.SetParent(ChartPool);
                break;  
            case E_ObjectPoolType.��ɫ���:
                SquarePoolManager.Instance.ReturnSquarePool(E_SquareType.ɫ��,instance, ColorSquarePool);
                break; 
            case E_ObjectPoolType.�ռ����:
                SquarePoolManager.Instance.ReturnSquarePool(E_SquareType.�ռ���,instance, CollectableSquarePool);
                break;
            case E_ObjectPoolType.�����������:
                SquarePoolManager.Instance.ReturnSquarePool(E_SquareType.����������,instance, TriggerRemoveSquarePool);
                break;

        }

        instance.SetActive(false);
    }

    /// <summary>
    /// ������
    /// </summary>
    public void FullWholePool(GameObject prefab, int poolCapcity, List<GameObject> pool, Transform poolFather)
    {
        //if (poolFather.childCount > 0)
        //{
        //    for (int i = 0; i < poolFather.childCount; ++i)
        //    {
        //        pool.Add(poolFather.GetChild(i).gameObject);
        //    }
        //}
        //Debug.Log(poolFather.name+ "������"+pool.Count+"����"+poolCapcity);
        for (int i = pool.Count; i < poolCapcity; i++)
            CreatNewInstance(prefab, pool, poolFather);
    }

    /// <summary>
    /// �ӳ���ȡ��һ��ʵ��
    /// </summary>
    /// <returns></returns>
    public GameObject GetInstnceFromPool(GameObject prefab, List<GameObject> pool, Transform poolFather)
    {
        GameObject instnce;
        //������д���δ����ķ��飬ȡ��һ������ʵ��
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                instnce = pool[i];
                instnce.SetActive(true);
                return instnce;
            }
        }
        //��������,������ʵ��
        instnce = CreatNewInstance(prefab, pool, poolFather);
        instnce.SetActive(true);
        return instnce;
    }

    /// <summary>
    /// ������ʵ��
    /// </summary>
    /// <returns></returns>
    public GameObject CreatNewInstance(GameObject prefab, List<GameObject> pool, Transform poolFather)
    {
        GameObject newInstance = Instantiate(prefab, poolFather);
        newInstance.SetActive(false);
        pool.Add(newInstance);
        return newInstance;
    }


}

public enum E_ObjectPoolType
{
    ��ҿ���������,
    ��ҿ��������,
    ��ҿ���ó�,
    ɫ��������ը��,
    
    ������,

    ��ɫ���,
    �ռ����,
    �����������,
    ���Ϳ�,

}


public enum E_SquareType
{
    ɫ��,
    �ռ���,
    ����������,
    ���Ϳ�,

}

public enum E_ParticalType
{
    ��ҿ�������,
    ��ҿ�������,
    ��ҿ����,
    ɫ��������ը,
}