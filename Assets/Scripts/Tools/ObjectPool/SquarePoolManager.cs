using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePoolManager : MonoSingleton<SquarePoolManager>
{
    WholeObjPoolManager poolManager;

    [Header("各色块数据集合")]
    public List<ColorSquareSO> prototyppeSODataList = new List<ColorSquareSO>();

    /// <summary>
    /// 随机颜色种子
    /// </summary>
    int randSeed;

    /// <summary>
    /// 记录so数据，存储在堆，减少gc
    /// </summary>
    ColorSquareSO newColorSo;

    protected override void InitSelf()
    {
        base.InitSelf();
        poolManager ??= WholeObjPoolManager.Instance;
    }

    public List<ColorSquareSO> GetColorSOList(List<int> intList)
    {
        List<ColorSquareSO> soList = new List<ColorSquareSO>();
        for (int i = 0; i < intList.Count; i++)
        {
            soList.Add(prototyppeSODataList[intList[i]]);
        }
        return soList;
    }

    /// <summary>
    /// 获得加工后的方块
    /// </summary>
    /// <param name="soData"></param>
    /// <returns></returns>
    public GameObject GetTargetSpecialSquare(E_SpecialSquareType  specialType)
    {
        GameObject colorSquare = poolManager.GetTargetSquareObj(E_SquareType.特殊块);

        switch (specialType)
        {
            case E_SpecialSquareType.消融方块:


                break;
            case
            
            E_SpecialSquareType.传送门方块:
                break;
            default:
                break;
        }

        StartCoroutine(AppearTrail(colorSquare));
        return colorSquare;
    }


    /// <summary>
    /// 获得加工后的方块
    /// </summary>
    /// <param name="soData"></param>
    /// <returns></returns>
    public GameObject GetTargetColorSquare(ColorSquareSO soData)
    {
        GameObject colorSquare = poolManager.GetTargetSquareObj(E_SquareType.色块);
        ColorWhiteSquare(colorSquare, soData);
        StartCoroutine(AppearTrail(colorSquare));
        return colorSquare;
    }


    /// <summary>
    /// 加工并返回一个随机颜色的色块
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomColorSquare()
    {
        GameObject colorSquare = poolManager.GetTargetSquareObj(E_SquareType.色块);
        //七种颜色随机
        randSeed = Random.Range(0, 6);
        newColorSo = prototyppeSODataList[randSeed];
        //染色
        ColorWhiteSquare(colorSquare, newColorSo);
        //添加尾迹
        StartCoroutine(AppearTrail(colorSquare));
        return colorSquare;
    }



    /// <summary>
    /// 加工操作-方块染色
    /// </summary>
    /// <param name="whiteSquare"></param>
    /// <param name="so"></param>
    void ColorWhiteSquare(GameObject whiteSquare, ColorSquareSO so)
    {
        whiteSquare.GetComponent<ColorSquare>().SetColorData(so);
    }

    /// <summary>
    /// 加工操作-添加尾迹
    /// </summary>
    /// <param name="colorSquare"></param>
    /// <returns></returns>
    IEnumerator AppearTrail(GameObject colorSquare)
    {
        colorSquare.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        colorSquare.transform.GetChild(0).gameObject.SetActive(true);
    }

    /// <summary>
    /// 方块重置入池
    /// </summary>
    public void ReturnSquarePool(E_SquareType squareType, GameObject square, Transform poolFather)
    {

        switch (squareType)
        {
            case E_SquareType.色块:
                square.transform.localScale = Vector3.zero;
                square.transform.SetParent(poolFather);

                //将层重置为Square
                square.gameObject.layer = 6;

                if (square.transform.childCount > 1)
                    DestroyImmediate(square.transform.GetChild(1).gameObject);

                //square.GetComponent<SpriteRenderer>().color = Color.white;
                square.GetComponent<ColorSquare>().myData = null;

                square.gameObject.SetActive(false);
                square.transform.localPosition = Vector3.zero;
                square.transform.localRotation = Quaternion.identity;
                square.transform.localScale = Vector3.one * 0.45f;
                break;

            case E_SquareType.特殊块:



                break;
            default:
                break;
        }
    }
}
