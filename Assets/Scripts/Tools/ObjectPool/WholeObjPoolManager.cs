using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    [Header("ɫ��Ԥ�Ƽ�")]
    private GameObject  colorSquarePrefab;
    [Header("�ռ���Ԥ�Ƽ�")]
    private GameObject collectableSquarePrefab;
    #endregion

    Transform PerfactPool;
    Transform NicePool;
    Transform GoodPool;
    Transform SquareExplodePool;
    Transform ChartPool;
    Transform ColorSquarePool;
    Transform CollectableSquarePool;

    //static private WholeObjPoolManager instance;
    //static public WholeObjPoolManager Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = FindAnyObjectByType<WholeObjPoolManager>();
    //            if (instance == null)
    //            {
    //                instance = new GameObject(typeof(WholeObjPoolManager) + "SingletonManager").AddComponent<WholeObjPoolManager>();
    //                instance.InitSelf();
    //            }
    //            DontDestroyOnLoad(instance.gameObject);
    //        }
    //        return instance;
    //    }
    //}
    //protected virtual void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Destroy(this);
    //        return;
    //    }
    //    instance = this;
    //    DontDestroyOnLoad(instance.gameObject);
    //}

    protected override void InitSelf()
    {
       PerfactPool       = Instantiate(new GameObject("perfactPool"),transform).transform;
       NicePool          = Instantiate(new GameObject("nicePool"), transform).transform;
       GoodPool          = Instantiate(new GameObject("goodPool"), transform).transform;
       SquareExplodePool = Instantiate(new GameObject("squareExplodePool"), transform).transform;
      
       perfactPrefab = Resources.Load<GameObject>("Prefab/Partical/Player_Hit_Perfact");
       nicePrefab = Resources.Load<GameObject>("Prefab/Partical/Player_Hit_Nice");
       goodPrefab = Resources.Load<GameObject>("Prefab/Partical/Player_Hit_Good");
       squareExplodetPrefab = Resources.Load<GameObject>("Prefab/Partical/Square_ExplodeEffect");
    
        FullWholePool(perfactPrefab,5,perfactPool, PerfactPool);
        FullWholePool(nicePrefab,5,nicePool, NicePool);
        FullWholePool(goodPrefab, 5,goodPool,GoodPool);
        FullWholePool(squareExplodetPrefab, 15, squareExplodePool, SquareExplodePool);


        ColorSquarePool = Instantiate(new GameObject("colorSquarePool"), transform).transform;
        CollectableSquarePool = Instantiate(new GameObject("collectableSquarePool"), transform).transform;
       
        colorSquarePrefab = Resources.Load<GameObject>("Prefab/Square/ColorSquare");
        collectableSquarePrefab = Resources.Load<GameObject>("Prefab/Square/CollectableSquare");

        FullWholePool(colorSquarePrefab, 90, colorSquarePool, ColorSquarePool);
        FullWholePool(collectableSquarePrefab, 10 , collectableSquarePool, CollectableSquarePool);


        ChartPool         = Instantiate(new GameObject("chartPool"), transform).transform;
        chartPrefab = Resources.Load<GameObject>("Prefab/Chart/Chart");
        FullWholePool(chartPrefab, chartPoolCapcity, chartPool, ChartPool);

    }
    
    public GameObject GetTargetSquareObj(E_SquareType squareType) 
    {
        switch (squareType)
        {
            case E_SquareType.ɫ��:
                return GetInstnceFromPool(colorSquarePrefab, colorSquarePool, ColorSquarePool); 
            case E_SquareType.�����:
                return GetInstnceFromPool(collectableSquarePrefab, collectableSquarePool, CollectableSquarePool); 
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
            case E_ObjectPoolType.ɫ���:
                SquarePoolManager.Instance.ReturnSquarePool(E_SquareType.ɫ��,instance, ColorSquarePool);
                break; 
            case E_ObjectPoolType.�����ռ����:
                SquarePoolManager.Instance.ReturnSquarePool(E_SquareType.�����,instance, CollectableSquarePool);
                break;

        }

        instance.SetActive(false);
    }





    /// <summary>
    /// ������
    /// </summary>
    public void FullWholePool(GameObject prefab, int poolCapcity, List<GameObject> pool, Transform poolFather)
    {
        for (int i = 0; i < poolCapcity; i++)
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

    ɫ���,
    �����ռ����
}


public enum E_SquareType
{
    ɫ��,
    �����
}

public enum E_ParticalType
{
    ��ҿ�������,
    ��ҿ�������,
    ��ҿ����,
    ɫ��������ը,
}