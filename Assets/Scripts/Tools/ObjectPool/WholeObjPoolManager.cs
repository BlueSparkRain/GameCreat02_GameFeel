using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WholeObjPoolManager : ObjectPool
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


    Transform PerfactPool;
    Transform NicePool;
    Transform GoodPool;
    Transform SquareExplodePool;
    Transform ChartPool;

    static private WholeObjPoolManager instance;
    static public WholeObjPoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<WholeObjPoolManager>();
                if (instance == null)
                {
                    instance = new GameObject(typeof(WholeObjPoolManager) + "SingletonManager").AddComponent<WholeObjPoolManager>();
                    instance.InitSelf();
                }
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(instance.gameObject);
    }

    protected void InitSelf()
    {
       PerfactPool       = Instantiate(new GameObject("perfactPool"),transform).transform;
       NicePool          = Instantiate(new GameObject("nicePool"), transform).transform;
       GoodPool          = Instantiate(new GameObject("goodPool"), transform).transform;
       SquareExplodePool = Instantiate(new GameObject("squareExplodePool"), transform).transform;
       ChartPool         = Instantiate(new GameObject("chartPool"), transform).transform;

        perfactPrefab = Resources.Load<GameObject>("Prefab/Partical/Player_Hit_Perfact");
        nicePrefab = Resources.Load<GameObject>("Prefab/Partical/Player_Hit_Nice");
        goodPrefab = Resources.Load<GameObject>("Prefab/Partical/Player_Hit_Good");
        squareExplodetPrefab = Resources.Load<GameObject>("Prefab/Partical/Square_ExplodeEffect");


        FullWholePool(perfactPrefab,5,perfactPool, PerfactPool);
        FullWholePool(nicePrefab,5,nicePool, NicePool);
        FullWholePool(goodPrefab, 5,goodPool,GoodPool);
        FullWholePool(squareExplodetPrefab, 15, squareExplodePool, SquareExplodePool);

        chartPrefab = Resources.Load<GameObject>("Prefab/Chart/Chart");
        FullWholePool(chartPrefab, chartPoolCapcity, chartPool, ChartPool);

    }
    
    public GameObject GetTargetPartical(E_ParticalType particalType) 
    {
        switch (particalType)
        {
            case E_ParticalType.玩家卡点完美:
                return GetInstnceFromPool(perfactPrefab, perfactPool, PerfactPool); ;
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

    public GameObject GetChart() 
    {
         return GetInstnceFromPool(chartPrefab,chartPool,ChartPool);
    }

    /// <summary>
    /// 方块重置入池
    /// </summary>
    public void ReturnPool(E_ObjectPoolType poolType,GameObject instance)
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
        }

        instance.SetActive(false);
    }
   
}
