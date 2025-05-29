using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePoolManager : MonoSingleton<SquarePoolManager>
{
    WholeObjPoolManager poolManager;

    [Header("色块数据集合")]
    public List<ColorSquareSO> prototyppe_Color_SODataList = new List<ColorSquareSO>();

    [Header("特殊块数据集合")]
    public List<SpecialSquareSO> prototyppe_Special_SODataList = new List<SpecialSquareSO>();
    
    /// <summary>
    /// 随机颜色种子
    /// </summary>
    int randSeed;

    /// <summary>
    /// 记录so数据，存储在堆，减少gc
    /// </summary>
    ColorSquareSO newColorSo;
    SpecialSquareSO newSpecialSo;

    protected override void InitSelf()
    {
        base.InitSelf();
        poolManager=FindAnyObjectByType<WholeObjPoolManager>();
    }

    public List<ColorSquareSO> GetColorSOList(List<int> intList)
    {
        List<ColorSquareSO> soList = new List<ColorSquareSO>();
        for (int i = 0; i < intList.Count; i++)
        {
            soList.Add(prototyppe_Color_SODataList[intList[i]]);
        }
        return soList;
    }

    /// <summary>
    ///  加工并返回一个特殊功能块
    /// </summary>
    /// <param name="specialRemoveType"></param>
    /// <returns></returns>
    public GameObject GetSpecialSquare(E_SpecialSquareType  specialRemoveType,int taskIndex)
    {
        GameObject specialSquare=null;
        switch (specialRemoveType)
        {
            case E_SpecialSquareType.消融收集:
                specialSquare = poolManager.GetTargetSquareObj(E_SquareType.收集块);
                newSpecialSo = prototyppe_Special_SODataList[0];
                break;
            case E_SpecialSquareType.触发消除:
                specialSquare = poolManager.GetTargetSquareObj(E_SquareType.任务消除块);
                specialSquare.GetComponent<TriggerRemovableSquare>().RemoveIndex=taskIndex;

                newSpecialSo = prototyppe_Special_SODataList[1];
                break;
            case E_SpecialSquareType.传送:
                specialSquare = poolManager.GetTargetSquareObj(E_SquareType.传送块);
                newSpecialSo = prototyppe_Special_SODataList[2];
                break;
        }

        SpriteSpecialSquare(specialSquare, newSpecialSo);
        StartCoroutine(AppearTrail(specialSquare));
        return specialSquare;
    }

    /// <summary>
    /// 加工并返回一个目标颜色的色块
    /// </summary>
    /// <param name="soData"></param>
    /// <returns></returns>
    public GameObject GetTargetColorSquare(ColorSquareSO soData)
    {
        InitSelf();
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

        //colorSquare.GetComponent<SquareController>().NewGame();

        randSeed = Random.Range(0, 6);
        newColorSo = prototyppe_Color_SODataList[randSeed];
        //染色
        ColorWhiteSquare(colorSquare, newColorSo);
        //添加尾迹
        StartCoroutine(AppearTrail(colorSquare));
        return colorSquare;
    }

    /// <summary>
    /// 加工操作-色块染色
    /// </summary>
    /// <param name="whiteSquare"></param>
    /// <param name="so"></param>
    void ColorWhiteSquare(GameObject whiteSquare, ColorSquareSO so)
    {
        whiteSquare.GetComponent<ColorSquare>().InitColorData(so);
    }


    /// <summary>
    /// 加工操作-特殊方块精灵
    /// </summary>
    /// <param name="specicalSquare"></param>
    /// <param name="so"></param>
    void SpriteSpecialSquare(GameObject specicalSquare,SpecialSquareSO so) 
    {
        specicalSquare.GetComponent<SpecicalSquare>().InitSpecialSquare(so);
    }

    /// <summary>
    /// 加工操作-添加尾迹
    /// </summary>
    /// <param name="colorSquare"></param>
    /// <returns></returns>
    IEnumerator AppearTrail(GameObject colorSquare)
    {

        colorSquare?.transform.GetChild(0).gameObject?.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        if (colorSquare)
        colorSquare?.transform.GetChild(0).gameObject?.SetActive(true);
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

                square.GetComponent<SpriteRenderer>().color = Color.white;
                square.GetComponent<ColorSquare>().myData = null;

                square.gameObject.SetActive(false);
                square.transform.localPosition = Vector3.zero;
                square.transform.localRotation = Quaternion.identity;
                square.transform.localScale = Vector3.one * 0.45f;
                break;

            case E_SquareType.收集块:



                break;
            default:
                break;
        }
    }
}
