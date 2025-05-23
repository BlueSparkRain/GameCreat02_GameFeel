using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WholeObjPoolManager :MonoSingleton<WholeObjPoolManager>
{
    #region 粒子池 Info
    public List<int> particlPoolsCapicity = new List<int>(4) {5,5,5,15};
    [Header("完美节拍池")]
    public List<GameObject> perfactPool = new List<GameObject>();
    [Header("优秀节拍池")]
    public List<GameObject> nicePool = new List<GameObject>();
    [Header("好好节拍池")]
    public List<GameObject> goodPool = new List<GameObject>();
    [Header("粒子爆炸节拍池")]
    public List<GameObject> squareExplodePool = new List<GameObject>();

    private GameObject perfactPrefab;
    private GameObject nicePrefab;
    private GameObject goodPrefab;
    private GameObject squareExplodetPrefab;
    #endregion

    #region 音符节拍池 Info
    [Header("音符池容量")]
    public int chartPoolCapcity = 5;
    [Header("音符节拍池")]
    public List<GameObject> chartPool = new List<GameObject>();
    [Header("音符预制件")]
    private GameObject chartPrefab;
    #endregion

    #region 方块池
    [Header("色块池")]
    public List<GameObject> colorSquarePool = new List<GameObject>();
    [Header("收集块池")]
    public List<GameObject> collectableSquarePool = new List<GameObject>();

    [Header("色块预制件")]
    private GameObject  colorSquarePrefab;
    [Header("收集块预制件")]
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
            case E_SquareType.色块:
                return GetInstnceFromPool(colorSquarePrefab, colorSquarePool, ColorSquarePool); 
            case E_SquareType.特殊块:
                return GetInstnceFromPool(collectableSquarePrefab, collectableSquarePool, CollectableSquarePool); 
            default:
                return GetInstnceFromPool(colorSquarePrefab, colorSquarePool, ColorSquarePool); 
               
        }
    }


    public GameObject GetTargetParticalObj(E_ParticalType particalType) 
    {
        switch (particalType)
        {
            case E_ParticalType.玩家卡点完美:
                return GetInstnceFromPool(perfactPrefab, perfactPool, PerfactPool); 
            case E_ParticalType.玩家卡点优秀:
                return GetInstnceFromPool(nicePrefab, nicePool, NicePool);
            case E_ParticalType.玩家卡点好:
                return GetInstnceFromPool(goodPrefab, goodPool, GoodPool);
            case E_ParticalType.色块消除爆炸:
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
    /// 方块重置入池
    /// </summary>
    public void ObjReturnPool(E_ObjectPoolType poolType,GameObject instance)
    {
        switch (poolType)
        {
            case E_ObjectPoolType.玩家卡点完美池:
                instance.transform.SetParent(PerfactPool);
                break;
            case E_ObjectPoolType.玩家卡点优秀池:
                instance.transform.SetParent(NicePool);
                break;
            case E_ObjectPoolType.玩家卡点好池:
                instance.transform.SetParent(GoodPool);
                break;
            case E_ObjectPoolType.色块消除爆炸池:
                instance.transform.SetParent(SquareExplodePool);
                break;
            case E_ObjectPoolType.音符池:
                instance.transform.SetParent(ChartPool);
                break;  
            case E_ObjectPoolType.色块池:
                SquarePoolManager.Instance.ReturnSquarePool(E_SquareType.色块,instance, ColorSquarePool);
                break; 
            case E_ObjectPoolType.道具收集块池:
                SquarePoolManager.Instance.ReturnSquarePool(E_SquareType.特殊块,instance, CollectableSquarePool);
                break;

        }

        instance.SetActive(false);
    }





    /// <summary>
    /// 充满池
    /// </summary>
    public void FullWholePool(GameObject prefab, int poolCapcity, List<GameObject> pool, Transform poolFather)
    {
        for (int i = 0; i < poolCapcity; i++)
            CreatNewInstance(prefab, pool, poolFather);
    }

    /// <summary>
    /// 从池中取出一个实例
    /// </summary>
    /// <returns></returns>
    public GameObject GetInstnceFromPool(GameObject prefab, List<GameObject> pool, Transform poolFather)
    {
        GameObject instnce;
        //如果池中存在未激活的方块，取出一个空闲实例
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                instnce = pool[i];
                instnce.SetActive(true);
                return instnce;
            }
        }
        //池中已满,生成新实例
        instnce = CreatNewInstance(prefab, pool, poolFather);
        instnce.SetActive(true);
        return instnce;
    }

    /// <summary>
    /// 创建新实例
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
    玩家卡点完美池,
    玩家卡点优秀池,
    玩家卡点好池,
    色块消除爆炸池,
    
    音符池,

    色块池,
    道具收集块池
}


public enum E_SquareType
{
    色块,
    特殊块
}

public enum E_ParticalType
{
    玩家卡点完美,
    玩家卡点优秀,
    玩家卡点好,
    色块消除爆炸,
}