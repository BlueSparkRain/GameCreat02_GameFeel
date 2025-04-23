using System.Collections.Generic;
using UnityEngine;

public class ChartObjPoolManager : MonoSingleton<ChartObjPoolManager>
{
    [Header("������")]
    public int poolCapcity=20;
    [Header("��������ȦԤ�Ƽ�")]
    [SerializeField] private GameObject ChartPrefab;

    List<GameObject> pool = new List<GameObject>();

    protected override void InitSelf()
    {
        base.InitSelf();
        ChartPrefab = Resources.Load<GameObject>("Prefab/Chart");
        FullWholePool();
    }

    /// <summary>
    /// ������
    /// </summary>
    void FullWholePool()
    {
        for (int i = 0; i < poolCapcity; i++)
            CreatNewInstance();
    }

    /// <summary>
    /// �ӳ���ȡ��һ������
    /// </summary>
    /// <returns></returns>
    public  GameObject GetChartInstnceFromPool()
    {
        GameObject instnce;
        //������д���δ����ķ��飬ȡ��һ����������
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                instnce = pool[i];
                instnce.SetActive(true);
                return instnce;
            }
        }
        //��������,����������
        instnce = CreatNewInstance();
        instnce.SetActive(true);
        return instnce;
    }

    /// <summary>
    /// �ڴ���ʵ��ʱ��
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
    /// �����������
    /// </summary>
    public void ReturnPool(Chart chart)
    { 
       
    }
}
