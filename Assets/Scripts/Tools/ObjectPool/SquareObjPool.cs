using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum E_ColorSquare 
{
 赤,橙,黄,绿,青,蓝,紫
}


public class SquareObjPool : MonoBehaviour
{
    [Header("池容量")]
    public int poolCapcity=81;
    public List<GameObject> pool = new List<GameObject>();

    [Header("各色块数据集合")]
    public List<ColorSquareSO> prototyppeSODataList=new List<ColorSquareSO>();

    [Header("无色方块预制件")]
    public GameObject WhiteSquarePrefab;

    void Start()
    {
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
    /// 加工并返回确定类型的色块
    /// </summary>
    /// <returns></returns>
    public GameObject GetTargetSquare(E_ColorSquare squareType) 
    {
        GameObject colorSquare = GetInstnce();
        ColorSquareSO so;

        switch (squareType)
        {
            case E_ColorSquare.赤:
                so = prototyppeSODataList[0];
                break;
            case E_ColorSquare.橙:
                so = prototyppeSODataList[1];
                break;
            case E_ColorSquare.黄:
                so = prototyppeSODataList[2];
                break;
            case E_ColorSquare.绿:
                so = prototyppeSODataList[3];
                break;
            case E_ColorSquare.青:
                so = prototyppeSODataList[4];
                break;
            case E_ColorSquare.蓝:
                so = prototyppeSODataList[5];
                break;
            case E_ColorSquare.紫:
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
    /// 加工并返回完成的色块
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomSquare() 
    {
        GameObject colorSquare = GetInstnce();
        //七种颜色随机
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
    /// 从池中返回一个实例
    /// </summary>
    /// <returns></returns>
    GameObject GetInstnce() 
    {
        GameObject instnce;
        //如果池中存在未激活的方块，取出一个空闲方块
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
        //池中已满,生成新方块
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
    /// 在创建实例时就
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
    /// 方块重置入池
    /// </summary>
    public void ReturnPool(ColorSquare square) 
    {
         //Debug.Log("回池+1");
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
