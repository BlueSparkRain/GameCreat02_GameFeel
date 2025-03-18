using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //如果池中存在未激活的方块，取出一个空闲方块
        for (int i = 0; i < pool.Count; i++) 
        {
            if (!pool[i].activeInHierarchy) 
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        //池中已满,生成新方块
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
         square.myData = null;
         square.GetComponent<SpriteRenderer>().color=Color.white;
         Debug.Log("回池+1");
         square.transform.SetParent(transform);
         square.gameObject.SetActive(false);
         square.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
         square.transform.localPosition = Vector3.zero;
         square.transform.localRotation = Quaternion.identity;
    }

  
}
