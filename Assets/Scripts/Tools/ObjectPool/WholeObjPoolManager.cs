using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    [Header("任务消除块池")]
    public List<GameObject> triggerRemoveSquarePool = new List<GameObject>();


    [Header("色块预制件")]
    private GameObject  colorSquarePrefab;
    [Header("收集块预制件")]
    private GameObject collectableSquarePrefab;
    [Header("任务消除块预制件")]
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
        Debug.Log("加载新的池子");

        perfactPool = new List<GameObject>();

        nicePool = new List<GameObject>();

        goodPool = new List<GameObject>();

        squareExplodePool = new List<GameObject>();


        chartPool = new List<GameObject>();

        colorSquarePool = new List<GameObject>();

        collectableSquarePool = new List<GameObject>();

        triggerRemoveSquarePool = new List<GameObject>();

        //找到场景中的GameMap,产生所有需要的池
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
    /// 返回色块或特殊快
    /// </summary>
    /// <param name="squareType"></param>
    /// <returns></returns>
    public GameObject GetTargetSquareObj(E_SquareType squareType) 
    {
        switch (squareType)
        {
            case E_SquareType.色块:

                return GetInstnceFromPool(colorSquarePrefab, colorSquarePool, ColorSquarePool); 
            case E_SquareType.收集块:
                return GetInstnceFromPool(collectableSquarePrefab, collectableSquarePool, CollectableSquarePool);
            case E_SquareType.任务消除块:
                return GetInstnceFromPool(triggerRemoveSquarePrefab, triggerRemoveSquarePool, TriggerRemoveSquarePool); 

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
            case E_ObjectPoolType.颜色块池:
                SquarePoolManager.Instance.ReturnSquarePool(E_SquareType.色块,instance, ColorSquarePool);
                break; 
            case E_ObjectPoolType.收集块池:
                SquarePoolManager.Instance.ReturnSquarePool(E_SquareType.收集块,instance, CollectableSquarePool);
                break;
            case E_ObjectPoolType.任务消除块池:
                SquarePoolManager.Instance.ReturnSquarePool(E_SquareType.任务消除块,instance, TriggerRemoveSquarePool);
                break;

        }

        instance.SetActive(false);
    }

    /// <summary>
    /// 充满池
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
        //Debug.Log(poolFather.name+ "池中有"+pool.Count+"还需"+poolCapcity);
        for (int i = pool.Count; i < poolCapcity; i++)
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

    颜色块池,
    收集块池,
    任务消除块池,
    传送块,

}


public enum E_SquareType
{
    色块,
    收集块,
    任务消除块,
    传送块,

}

public enum E_ParticalType
{
    玩家卡点完美,
    玩家卡点优秀,
    玩家卡点好,
    色块消除爆炸,
}