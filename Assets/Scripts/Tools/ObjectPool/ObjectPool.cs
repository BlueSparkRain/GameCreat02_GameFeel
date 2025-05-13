using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool:MonoBehaviour
{

    /// <summary>
    /// ������
    /// </summary>
    public void FullWholePool(GameObject prefab,int poolCapcity, List<GameObject> pool, Transform poolFather)
    {
        for (int i = 0; i < poolCapcity; i++)
            CreatNewInstance(prefab,pool, poolFather);
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
    ��ҿ���������,
    ��ҿ��������,
    ��ҿ���ó�,
    ɫ��������ը��,
    ������
}
