using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameRow : MonoBehaviour
{
    public List<Square> rowSquares;
    GameCol col;

    List<Square> toRemoveSquares = new List<Square>();


    /// <summary>
    /// 在目标槽处产生特殊方块
    /// </summary>
    /// <param name="so"></param>
    /// <param name="slotIndex"></param>
    IEnumerator MakeSuperSquare(Square SuperSquare)
    {
        yield return SuperSquare.SquareMoveAnim();
        var superEffect = Instantiate(Resources.Load<GameObject>("Prefab/SuperMark/SuperMark"));
        superEffect.transform.SetParent(SuperSquare.transform);
        superEffect.transform.localPosition = Vector3.zero;
    }




    public void UpdateRowSquare(Square square, int index)
    {
        if (rowSquares != null)
        {
            rowSquares[index] = square;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slotCapcity"></param>
    public void InitRow(int slotCapcity) 
    {
        rowSquares = new List<Square>();
        col=GetComponent<GameCol>();
        for (int i = 0; i < slotCapcity; i++)
            rowSquares.Add(null);
    }

    public bool isRemoving;

    WaitForSeconds removeDelay = new WaitForSeconds(0.1f);

  

    float checkRemoveTimer;
    float checkRemoveInterval=0.2f;
    bool canCheckRemove;

    void Update()
    {
        if (!canCheckRemove && checkRemoveTimer >= 0)
            checkRemoveTimer -= Time.deltaTime;
        else
            canCheckRemove = true;

        if (canCheckRemove && rowSquares != null)
        {
            RemoveSquares();
            canCheckRemove=false;
            checkRemoveTimer = checkRemoveInterval;
        }
    }

    public void RemoveSquares()
    {
        if (CheckRemoveList() != null)
            StartCoroutine(CheckAndRemoveSquares(CheckRemoveList()));
    }

    int firstIndex;
    E_Color firstCor;

    int num = 1;
    public List<Square> CheckRemoveList()
    {
        toRemoveSquares.Clear();
        num = 1;
        bool canAdd = true;


        for (int i = 0; i < rowSquares.Count; ++i)
        {
            if (rowSquares[i] == null ||
                !rowSquares[i].GetComponent<ColorSquare>() ||
                !rowSquares[i].GetComponent<ColorSquare>().myData)
                continue; 
           firstCor=rowSquares[i].GetComponent<ColorSquare>().myData.E_Color;
           firstIndex = i;
           break; 
        }

        //如果本列剩余少于两个色块了，一定无法消除，返回
        if (firstIndex >= rowSquares.Count - 2)
            return null;

        //记录消除检测起点
        toRemoveSquares.Add(rowSquares[firstIndex]);

        for (int i = firstIndex + 1; i < rowSquares.Count; ++i)
        { 
            if (!rowSquares[i] || !rowSquares[i].GetComponent<ColorSquare>())
            {

                if (toRemoveSquares.Count >= 3)
                    return toRemoveSquares;

                num = 0;//Test
                continue;
            }

            if (rowSquares[i].GetComponent<ColorSquare>().myData != null)
            {

                if (rowSquares[i].GetComponent<ColorSquare>().myData.E_Color == firstCor
                    && canAdd)
                {
                    if (!toRemoveSquares.Contains(rowSquares[i]))
                    {
                        toRemoveSquares.Add(rowSquares[i]);
                        num++;
                    }
                }
                else
                {
                    if (num < 3)
                    {
                        firstCor = rowSquares[i].GetComponent<ColorSquare>().myData.E_Color;
                        toRemoveSquares.Clear();
                        canAdd = true;
                        toRemoveSquares.Add(rowSquares[i]);
                        num = 1;
                    }
                    else
                    {

                        canAdd = false;
                    }
                }
            }
        }
        if (toRemoveSquares.Count >= 3)
            return toRemoveSquares;
        else
            return null;
    }

    IEnumerator CheckAndRemoveSquares(List<Square> removeLists)
    {

        if (!GetComponentInChildren<SubCol>().isRemoving && !isRemoving)
        {
            IsRowRemoving();
            if (removeLists.Count <= 2)
            {

            }
            else if (removeLists.Count >= 5)
            {
                yield return RemoveRowLine(removeLists, RemoveRowLine5);
            }
            else if (removeLists.Count >= 4)
            {
                yield return RemoveRowLine(removeLists, RemoveRowLine4);
            }

            else if (removeLists.Count >= 3)
            {
                yield return RemoveRowLine(removeLists, RemoveRowLine3);
            }

            StopRowRemoving();
        }
    }

    /// <summary>
    /// 本行开始消除任务
    /// </summary>
    void IsRowRemoving()
    {
        isRemoving = true;
    }
    /// <summary>
    /// 本行完成消除任务
    /// </summary>
    void StopRowRemoving()
    {
        isRemoving = false;
    }

    /// <summary>
    /// 连线3消：无功能，只积分
    /// </summary>
    public void RemoveRowLine3()
    {
        Debug.Log("完成3消");

        //5消机制
    }


    /// <summary>
    /// 连线4消：消除+生成1个整列炸弹色块
    /// </summary>
    public void RemoveRowLine4()
    {
        Debug.Log("完成4消");

        //5消机制
    }


    /// <summary>
    /// 连线5消：消除+生成色块闪电：清除所有相同颜色色块
    /// </summary>
    public void RemoveRowLine5()
    {
        Debug.Log("完成5消");

        //5消机制
    }

    IEnumerator RemoveRowLine(List<Square> toRemoveSquares, UnityAction callback = null)
    {
        Square SuperSquare = null;
        int superSquareIndex=0;
        if (toRemoveSquares.Count >= 3)
        {
            superSquareIndex = Random.Range(0, toRemoveSquares.Count);
            while (toRemoveSquares[superSquareIndex].GetComponent<PlayerController>())
            {
                superSquareIndex = Random.Range(1, toRemoveSquares.Count);
            }

            SuperSquare = toRemoveSquares[superSquareIndex];
        }


        for (int i = 0; i < toRemoveSquares.Count; i++)
        {
            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                StartCoroutine(toRemoveSquares[i].BeRemoved());
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                //i++;
                continue;
            }

            if (toRemoveSquares.Count >= 3 && i == superSquareIndex)
            {
                Debug.Log("太六了" + superSquareIndex + "-" + SuperSquare);
                //yield return removeDelay;
                continue;
            }

            if (toRemoveSquares[i].transform.parent != null)
            {
                SubCol targetCol = toRemoveSquares[i].transform.GetComponentInParent<SubCol>(); // parent?.parent?.GetComponent<GameCol>();
                //targetCol?.IsColumnRemoving();
                StartCoroutine(toRemoveSquares[i].BeRemoved());

                targetCol?.ColPrepareNeededSquare();
                StartCoroutine(WaitColRemove(targetCol));

                yield return removeDelay;
                
            }

            if (SuperSquare != null)
            {
                Debug.Log(SuperSquare);
                StartCoroutine(MakeSuperSquare(SuperSquare));
            }


        }
        callback?.Invoke();
    }

    IEnumerator WaitColRemove(SubCol targetCol) 
    {
        targetCol?.IsColumnRemoving();
        yield return new WaitForSeconds(0.4f);
        targetCol?.StopColumnRemoving();
    }
}
