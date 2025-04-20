using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum E_ColorSquare 
{
 ��,��,��,��,��,��,��
}


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

    public List<ColorSquareSO> GetColorSOList(List<int> intList) 
    { 
      List<ColorSquareSO> soList = new List<ColorSquareSO>();
      for(int i = 0;i < intList.Count; i++) 
      {
            soList.Add(prototyppeSODataList[intList[i]]);
      }
      return soList;
    }


    public GameObject GetTargetSquare(ColorSquareSO soData)
    {
        GameObject colorSquare = GetInstnce();
        ColorWhiteSquare(colorSquare, soData);
        StartCoroutine(AppearTrail(colorSquare));
        return colorSquare;
    }


    /// <summary>
    /// �ӹ�������ȷ�����͵�ɫ��
    /// </summary>
    /// <returns></returns>
    public GameObject GetTargetSquare(E_ColorSquare squareType) 
    {
        GameObject colorSquare = GetInstnce();
        ColorSquareSO so;

        switch (squareType)
        {
            case E_ColorSquare.��:
                so = prototyppeSODataList[0];
                break;
            case E_ColorSquare.��:
                so = prototyppeSODataList[1];
                break;
            case E_ColorSquare.��:
                so = prototyppeSODataList[2];
                break;
            case E_ColorSquare.��:
                so = prototyppeSODataList[3];
                break;
            case E_ColorSquare.��:
                so = prototyppeSODataList[4];
                break;
            case E_ColorSquare.��:
                so = prototyppeSODataList[5];
                break;
            case E_ColorSquare.��:
                so = prototyppeSODataList[6];
                break;
            default:
                so = prototyppeSODataList[0];
                break;
        }

        ColorWhiteSquare(colorSquare, so);
        StartCoroutine(AppearTrail(colorSquare));
        return colorSquare;
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
        GameObject instnce;
        //������д���δ����ķ��飬ȡ��һ�����з���
        for (int i = 0; i < pool.Count; i++) 
        {
            if (!pool[i].activeInHierarchy) 
            {
                instnce = pool[i];
                //pool.RemoveAt(i);
                instnce.SetActive(true);
                return  instnce;
            }
        }
        //��������,�����·���
        instnce=CreatNewInstance();
        //pool.Remove(instnce);
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
         //Debug.Log("�س�+1");
         //pool.Add(square.gameObject);
         square.transform.localScale = Vector3.one*1.6f;
         square.gameObject.SetActive(false);
         square.transform.SetParent(transform);
         square.GetComponent<SpriteRenderer>().color=Color.white;
         square.myData = null;
         square.transform.localPosition = Vector3.zero;
         square.transform.localRotation = Quaternion.identity;
    }
}
