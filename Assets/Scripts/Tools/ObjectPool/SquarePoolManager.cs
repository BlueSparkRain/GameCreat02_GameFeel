using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePoolManager : MonoSingleton<SquarePoolManager>
{
    WholeObjPoolManager poolManager;

    [Header("ɫ�����ݼ���")]
    public List<ColorSquareSO> prototyppe_Color_SODataList = new List<ColorSquareSO>();

    [Header("��������ݼ���")]
    public List<SpecialSquareSO> prototyppe_Special_SODataList = new List<SpecialSquareSO>();
    
    /// <summary>
    /// �����ɫ����
    /// </summary>
    int randSeed;

    /// <summary>
    /// ��¼so���ݣ��洢�ڶѣ�����gc
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
    ///  �ӹ�������һ�����⹦�ܿ�
    /// </summary>
    /// <param name="specialRemoveType"></param>
    /// <returns></returns>
    public GameObject GetSpecialSquare(E_SpecialSquareType  specialRemoveType,int taskIndex)
    {
        GameObject specialSquare=null;
        switch (specialRemoveType)
        {
            case E_SpecialSquareType.�����ռ�:
                specialSquare = poolManager.GetTargetSquareObj(E_SquareType.�ռ���);
                newSpecialSo = prototyppe_Special_SODataList[0];
                break;
            case E_SpecialSquareType.��������:
                specialSquare = poolManager.GetTargetSquareObj(E_SquareType.����������);
                specialSquare.GetComponent<TriggerRemovableSquare>().RemoveIndex=taskIndex;

                newSpecialSo = prototyppe_Special_SODataList[1];
                break;
            case E_SpecialSquareType.����:
                specialSquare = poolManager.GetTargetSquareObj(E_SquareType.���Ϳ�);
                newSpecialSo = prototyppe_Special_SODataList[2];
                break;
        }

        SpriteSpecialSquare(specialSquare, newSpecialSo);
        StartCoroutine(AppearTrail(specialSquare));
        return specialSquare;
    }

    /// <summary>
    /// �ӹ�������һ��Ŀ����ɫ��ɫ��
    /// </summary>
    /// <param name="soData"></param>
    /// <returns></returns>
    public GameObject GetTargetColorSquare(ColorSquareSO soData)
    {
        InitSelf();
        GameObject colorSquare = poolManager.GetTargetSquareObj(E_SquareType.ɫ��);
        ColorWhiteSquare(colorSquare, soData);
        StartCoroutine(AppearTrail(colorSquare));
        return colorSquare;
    }

    /// <summary>
    /// �ӹ�������һ�������ɫ��ɫ��
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomColorSquare()
    {
        GameObject colorSquare = poolManager.GetTargetSquareObj(E_SquareType.ɫ��);
        //������ɫ���

        //colorSquare.GetComponent<SquareController>().NewGame();

        randSeed = Random.Range(0, 6);
        newColorSo = prototyppe_Color_SODataList[randSeed];
        //Ⱦɫ
        ColorWhiteSquare(colorSquare, newColorSo);
        //���β��
        StartCoroutine(AppearTrail(colorSquare));
        return colorSquare;
    }

    /// <summary>
    /// �ӹ�����-ɫ��Ⱦɫ
    /// </summary>
    /// <param name="whiteSquare"></param>
    /// <param name="so"></param>
    void ColorWhiteSquare(GameObject whiteSquare, ColorSquareSO so)
    {
        whiteSquare.GetComponent<ColorSquare>().InitColorData(so);
    }


    /// <summary>
    /// �ӹ�����-���ⷽ�龫��
    /// </summary>
    /// <param name="specicalSquare"></param>
    /// <param name="so"></param>
    void SpriteSpecialSquare(GameObject specicalSquare,SpecialSquareSO so) 
    {
        specicalSquare.GetComponent<SpecicalSquare>().InitSpecialSquare(so);
    }

    /// <summary>
    /// �ӹ�����-���β��
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
    /// �����������
    /// </summary>
    public void ReturnSquarePool(E_SquareType squareType, GameObject square, Transform poolFather)
    {

        switch (squareType)
        {
            case E_SquareType.ɫ��:
                square.transform.localScale = Vector3.zero;
                square.transform.SetParent(poolFather);

                //��������ΪSquare
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

            case E_SquareType.�ռ���:



                break;
            default:
                break;
        }
    }
}
