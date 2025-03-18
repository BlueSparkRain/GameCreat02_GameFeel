using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareObjPool : MonoBehaviour
{
    [Header("������")]
    public int poolCapcity=81;
    public List<GameObject> pool = new List<GameObject>();

    [Header("��ɫ�����ݼ���")]
    public List<ColorSquareSO> prototyppeSODataList=new List<ColorSquareSO>();

    [Header("��ɫ����Ԥ�Ƽ�")]
    public GameObject WhiteSquarePrefab;

    void Start()
    {
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
    /// �ӹ���������ɵ�ɫ��
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomSquare() 
    {
        GameObject colorSquare = GetInstnce();
        //������ɫ���
        int randSeed = Random.Range(0, 6);
        ColorSquareSO so = prototyppeSODataList[randSeed];
        ColorWhiteSquare(colorSquare,so);

        StartCoroutine(AppearTrail(colorSquare));
        return colorSquare;
    }

    IEnumerator AppearTrail(GameObject colorSquare) 
    {
        colorSquare.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        colorSquare.transform.GetChild(0).gameObject.SetActive(true);
    }

    /// <summary>
    /// �ӳ��з���һ��ʵ��
    /// </summary>
    /// <returns></returns>
    GameObject GetInstnce() 
    {
        //������д���δ����ķ��飬ȡ��һ�����з���
        for (int i = 0; i < pool.Count; i++) 
        {
            if (!pool[i].activeInHierarchy) 
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        //��������,�����·���
        GameObject instnce=CreatNewInstance();
        instnce.SetActive(true);
        return instnce;
    }

    void ColorWhiteSquare(GameObject whiteSquare,ColorSquareSO so) 
    {

        whiteSquare.GetComponent<ColorSquare>().myData = so;
        whiteSquare.GetComponent<ColorSquare>().ColorSelf();
    }

    /// <summary>
    /// �ڴ���ʵ��ʱ��
    /// </summary>
    /// <returns></returns>
    GameObject CreatNewInstance() 
    {
      GameObject newInstance=Instantiate(WhiteSquarePrefab,transform);
      newInstance.SetActive(false);
      pool.Add(newInstance);
      return newInstance;
    }


    /// <summary>
    /// �����������
    /// </summary>
    public void ReturnPool(ColorSquare square) 
    {
         square.myData = null;
         square.GetComponent<SpriteRenderer>().color=Color.white;
         Debug.Log("�س�+1");
         square.transform.SetParent(transform);
         square.gameObject.SetActive(false);
         square.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
         square.transform.localPosition = Vector3.zero;
         square.transform.localRotation = Quaternion.identity;
    }

  
}
