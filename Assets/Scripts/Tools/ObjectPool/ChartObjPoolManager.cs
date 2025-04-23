using System.Collections.Generic;
using UnityEngine;

public class ChartObjPoolManager : MonoSingleton<ChartObjPoolManager>
{
    [Header("池容量")]
    public int poolCapcity=20;
    [Header("节拍收缩圈预制件")]
    [SerializeField] private GameObject ChartPrefab;

    List<GameObject> pool = new List<GameObject>();

    protected override void InitSelf()
    {
        base.InitSelf();
        ChartPrefab = Resources.Load<GameObject>("Prefab/Chart");
        FullWholePool();
    }

    /// <summary>
    /// 充满池
    /// </summary>
    void FullWholePool()
    {
        for (int i = 0; i < poolCapcity; i++)
            CreatNewInstance();
    }

    /// <summary>
    /// 从池中取出一个音符
    /// </summary>
    /// <returns></returns>
    public  GameObject GetChartInstnceFromPool()
    {
        GameObject instnce;
        //如果池中存在未激活的方块，取出一个空闲音符
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                instnce = pool[i];
                instnce.SetActive(true);
                return instnce;
            }
        }
        //池中已满,生成新音符
        instnce = CreatNewInstance();
        instnce.SetActive(true);
        return instnce;
    }

    /// <summary>
    /// 在创建实例时就
    /// </summary>
    /// <returns></returns>
    GameObject CreatNewInstance()
    {
        GameObject newInstance = Instantiate(ChartPrefab,transform);
        newInstance.SetActive(false);
        pool.Add(newInstance);
        return newInstance;
    }

    /// <summary>
    /// 方块重置入池
    /// </summary>
    public void ReturnPool(Chart chart)
    { 
       
    }
}
