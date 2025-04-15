using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SquareRow : MonoBehaviour
{
    public List<Square> rowSquares = new List<Square>();

    public int SquareIndex { get; private set; }
    [Header("本行方块")]
    int SquareNum;//尚未使用 未来可期

    public bool RowFull;

    public bool isRemoving;

    private float removeInterval = 0.3f;


    /// <summary>
    /// 消除本行所有Square
    /// </summary>
    /// <param name="RowIndex"></param>
    /// <returns></returns>
    public IEnumerator RemoveWholeRow()
    {
        for (int i = 0; i < rowSquares.Count; i++)
        {
            if (rowSquares[i].GetComponent<PlayerController>())
            {
                if (i + 1 >= rowSquares.Count)
                    yield break;
                i++;
            }

            Transform targetCol = rowSquares[i].transform?.parent.parent;
            yield return rowSquares[i].BeRemoved();
            targetCol?.GetComponent<SquareColumn>().ColumnAddOneSquare();
            yield return new WaitForSeconds(removeInterval);
        }
    }

    public void SetRowSquare(Square square, int index)
    {
        rowSquares[index] = square;
        //Debug.Log(index + "初始化行更新");
        UpdateRowFullState();
    }


    /// <summary>
    /// 更新本行是否已满
    /// </summary>
    public void UpdateRowFullState()
    {
        for (int i = 0; i < rowSquares.Count; i++)
        {
            if (!rowSquares[i])
            {
                RowFull = false;
                break;
            }
            RowFull = true;
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

    private void Update()
    {
        if (RowFull)
            RemoveSquares();
    }

    public void RemoveSquares()
    {
        if (CheckRemoveList() != null)
            StartCoroutine(CheckAndRemoveSquares(CheckRemoveList()));
    }
    int startCheckIndex;
    public List<Square> CheckRemoveList()
    {
        int num = 1;
        bool canAdd = true;
        List<Square> toRemoveSquares = new List<Square>();


        startCheckIndex = 0;

        for (int i = 0; i < 8; i++)
        {
            if (rowSquares[i] != null && rowSquares[i].GetComponent<ColorSquare>() && rowSquares[i].GetComponent<ColorSquare>().myData)
            {
                startCheckIndex = i;
                break;
            }
        }

        E_Color firstCor = rowSquares[startCheckIndex].GetComponent<ColorSquare>().myData.E_Color;
        toRemoveSquares.Add(rowSquares[startCheckIndex]);

        for (int i = startCheckIndex + 1; i < rowSquares.Count; i++)
        {
            if (!rowSquares[i] || !rowSquares[i].GetComponent<ColorSquare>())
            {
                continue;
            }

            if (!rowSquares[i].GetComponent<ColorSquare>().myData || !rowSquares[i])
                return null;

            if (rowSquares[i] != null && rowSquares[i].GetComponent<ColorSquare>() && rowSquares[i].GetComponent<ColorSquare>().myData.E_Color == firstCor && canAdd)
            {
                if (!toRemoveSquares.Contains(rowSquares[i]))
                {
                    num++;
                    toRemoveSquares.Add(rowSquares[i]);
                }
            }
            else
            {
                if (num < 3)
                {
                    firstCor = rowSquares[i].GetComponent<ColorSquare>().myData.E_Color;
                    toRemoveSquares.Clear();
                    num = 1;
                    canAdd = true;
                    toRemoveSquares.Add(rowSquares[i]);
                }
                else
                {
                    canAdd = false;
                    toRemoveSquares.Remove(rowSquares[i]);
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

        if (!GetComponent<SquareColumn>().isRemoving && !isRemoving)
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

    IEnumerator RemoveRowLine(List<Square> toRemoveSquares,UnityAction callback=null)
    {
        for (int i = 0; i < toRemoveSquares.Count; i++)
        {
            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                StartCoroutine(toRemoveSquares[i].BeRemoved());
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                i++;
            }

            if (toRemoveSquares[i].transform.parent != null)
            {
                SquareColumn targetCol = toRemoveSquares[i].transform?.parent?.parent?.GetComponent<SquareColumn>();
                targetCol?.IsColumnRemoving();
                StartCoroutine(toRemoveSquares[i].BeRemoved());


                StartCoroutine(WaitRemoveSpawen(targetCol));
                yield return new WaitForSeconds(removeInterval);
                //targetCol?.ColumnAddOneSquare();
                targetCol?.StopColumnRemoving();
            }
        }

        callback?.Invoke();
    }


    IEnumerator WaitRemoveSpawen(SquareColumn col)
    {
        yield return new WaitForSeconds(0.8f);
        col?.ColumnAddOneSquare();

    }

}
