using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool:MonoBehaviour
{

    /// <summary>
    /// 充满池
    /// </summary>
    public void FullWholePool(GameObject prefab,int poolCapcity, List<GameObject> pool, Transform poolFather)
    {
        for (int i = 0; i < poolCapcity; i++)
            CreatNewInstance(prefab,pool, poolFather);
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
    public GameObject CreatNewInstance(GameObject prefab, List<GameObject> pool,Transform poolFather)
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
    音符池
}
