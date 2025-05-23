using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePoolManager : MonoSingleton<SquarePoolManager>
{
    WholeObjPoolManager poolManager;

    [Header("��ɫ�����ݼ���")]
    public List<ColorSquareSO> prototyppeSODataList = new List<ColorSquareSO>();

    /// <summary>
    /// �����ɫ����
    /// </summary>
    int randSeed;

    /// <summary>
    /// ��¼so���ݣ��洢�ڶѣ�����gc
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
    /// ��üӹ���ķ���
    /// </summary>
    /// <param name="soData"></param>
    /// <returns></returns>
    public GameObject GetTargetSpecialSquare(E_SpecialSquareType  specialType)
    {
        GameObject colorSquare = poolManager.GetTargetSquareObj(E_SquareType.�����);

        switch (specialType)
        {
            case E_SpecialSquareType.���ڷ���:


                break;
            case
            
            E_SpecialSquareType.�����ŷ���:
                break;
            default:
                break;
        }

        StartCoroutine(AppearTrail(colorSquare));
        return colorSquare;
    }


    /// <summary>
    /// ��üӹ���ķ���
    /// </summary>
    /// <param name="soData"></param>
    /// <returns></returns>
    public GameObject GetTargetColorSquare(ColorSquareSO soData)
    {
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
        randSeed = Random.Range(0, 6);
        newColorSo = prototyppeSODataList[randSeed];
        //Ⱦɫ
        ColorWhiteSquare(colorSquare, newColorSo);
        //���β��
        StartCoroutine(AppearTrail(colorSquare));
        return colorSquare;
    }



    /// <summary>
    /// �ӹ�����-����Ⱦɫ
    /// </summary>
    /// <param name="whiteSquare"></param>
    /// <param name="so"></param>
    void ColorWhiteSquare(GameObject whiteSquare, ColorSquareSO so)
    {
        whiteSquare.GetComponent<ColorSquare>().SetColorData(so);
    }

    /// <summary>
    /// �ӹ�����-���β��
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

                //square.GetComponent<SpriteRenderer>().color = Color.white;
                square.GetComponent<ColorSquare>().myData = null;

                square.gameObject.SetActive(false);
                square.transform.localPosition = Vector3.zero;
                square.transform.localRotation = Quaternion.identity;
                square.transform.localScale = Vector3.one * 0.45f;
                break;

            case E_SquareType.�����:



                break;
            default:
                break;
        }
    }
}
