using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SquareColumn : MonoBehaviour
{
    [Header("方块增殖器")]
    public Transform squareSpawner;
    [Header("最上层方块位序")]
    public int SquareIndex { get; private set; }
    [Header("本列方块")]
    public int SquareNum;
    [Header("玩家出生信息")]
    public PlayerBornData playerBornData;

    public List<Square> columnSquares = new List<Square>();
    public int FirstEmptySlotIndex;

    private List<Square> toRemoveSquares = new List<Square>();

    public bool ColFull;

    public bool isRemoving;

    private float spawnInterval = 0.15f;

    /// <summary>
    /// 更新本列方块
    /// </summary>
    /// <param name="square"></param>
    /// <param name="index"></param>
    public void UpdateColumnSquares(Square square, int index)
    {
        columnSquares[index] = square;

        for (int i = 0; i < columnSquares.Count; i++)
        {
            if (!columnSquares[i])
            {
                ColFull = false;
                break;
            }
            ColFull = true;
        }


        if (!ColFull)
            return;

        if (SquareNum == 8 && FirstEmptySlotIndex != 0)
        {
            for (int i = 0; i < FirstEmptySlotIndex; i++)
                StartCoroutine(ColumnAddOneSquare());
        }

        for (int i = 1; i < columnSquares.Count; i++)
        {
            if (!columnSquares[i] || !columnSquares[i].GetComponent<ColorSquare>())
                return;
        }
        CheckColCanRemove();

    }
    bool canCheck=true;
    private void Update()
    {

    
    }

    public void CheckColCanRemove()
    {
        if (GetComponent<SquareRow>().isRemoving || !columnSquares[0]|| isRemoving)
            return;

        int num = 1;
        bool canAdd=true;
        toRemoveSquares.Clear();

        E_Color firstCor = columnSquares[0].GetComponent<ColorSquare>().myData.E_Color;
        toRemoveSquares.Add(columnSquares[0]);

        for (int i = 1; i < columnSquares.Count; i++)
        {
            if (columnSquares[i] && columnSquares[i].GetComponent<ColorSquare>() && columnSquares[i].GetComponent<ColorSquare>().myData.E_Color == firstCor && canAdd)
            {
               
                if (!toRemoveSquares.Contains(columnSquares[i]))
                {
                    num++;
                    toRemoveSquares.Add(columnSquares[i]);
                }
               
                //Debug.Log(firstCor + "同色" + num);
            }
            else
            {
                if (num < 3)
                {
                    firstCor = columnSquares[i].GetComponent<ColorSquare>().myData.E_Color;
                    toRemoveSquares.Clear();
                    num = 1;
                    canAdd = true;
                    toRemoveSquares.Add(columnSquares[i]);
                }
                else
                {
                    canAdd = false;
                    toRemoveSquares.Remove(columnSquares[i]);
                }
            }
        }
        //Debug.Log("本行最多:"+ firstCor.ToString()+num);
        //遍历完整列，执行组合消除

        StartCoroutine(RemoveSquares(num));
    }

    IEnumerator RemoveSquares(int num)
    {
        if (!isRemoving)
        {
            isRemoving = true;
            if (num <= 2)
            {

            }
            else if (num >= 5)
            {
                yield return RemoveColLine5();
            }
            else if (num >= 4)
            {
                yield return RemoveColLine4();
            }

            else if (num >= 3)
            {
                yield return RemoveColLine3();
            }
            //yield return new  WaitForSeconds(0.2f);
            isRemoving = false;
        }
    }

    /// <summary>
    /// 连线3消：无功能，只积分
    /// </summary>
    public IEnumerator RemoveColLine3()
    {
        Debug.Log("完成3消");

        for (int i = 0; i < toRemoveSquares.Count; i++)
        {
            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                i++;
            }
            yield return toRemoveSquares[i].BeRemoved();
            yield return ColumnAddOneSquare();
            yield return new WaitForSeconds(spawnInterval);
        }

    }

    /// <summary>
    /// 连线4消：消除+生成1个整列炸弹色块
    /// </summary>
    public IEnumerator RemoveColLine4()
    {
        Debug.Log("完成4消");
        for (int i = 0; i < toRemoveSquares.Count; i++)
        {
            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                i++;
            }
            yield return toRemoveSquares[i].BeRemoved();
            yield return ColumnAddOneSquare();
            yield return new WaitForSeconds(spawnInterval);
        }

    }

    /// <summary>
    /// 连线5消：消除+生成色块闪电：清除所有相同颜色色块
    /// </summary>
    public IEnumerator RemoveColLine5()
    {
        Debug.Log("完成5消");
        for (int i = 0; i < toRemoveSquares.Count; i++)
        {
            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                i++;
            }
            yield return toRemoveSquares[i].BeRemoved();
            yield return ColumnAddOneSquare();
            yield return new WaitForSeconds(spawnInterval);
        }

    }

    /// <summary>
    /// 清除整列Square
    /// </summary>
    /// <returns></returns>
    public IEnumerator RemoveWholeColumn()
    {
        int removeIndex = columnSquares.Count - 1;

        for (int i = columnSquares.Count - 1; i >= 0; i--)
        {
            if (columnSquares[columnSquares.Count - 1].GetComponent<PlayerController>())
                removeIndex = columnSquares.Count - 2;

            yield return columnSquares[removeIndex].BeRemoved();
            yield return ColumnAddOneSquare();
            yield return new WaitForSeconds(0.02f);
        }
    }




    void Start()
    {
        SquareIndex = 7;
        for (int i = 0; i < 8; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(SquareIndex).gameObject.SetActive(true);

        squareSpawner = transform.GetChild(8);
    }

    /// <summary>
    /// 更新最上层的空槽索引
    /// </summary>
    /// <param name="index"></param>
    public void UpdateTopSlot(int index)
    {
        GetOneSquare();
        FirstEmptySlotIndex = index;

        if (ColFull && FirstEmptySlotIndex > 0 && SquareNum <= 8)
        {
            int toBorn = FirstEmptySlotIndex;
            for (int i = 0; i < toBorn; i++)
            {
                if (ColFull && transform.GetChild(i).childCount == 0 && columnSquares[i] == null)
                {
                    StartCoroutine(ColumnAddOneSquare());
                }
            }
        }

        if (ColFull && SquareNum == 8 && FirstEmptySlotIndex != 0)
        {
            for (int i = 0; i < FirstEmptySlotIndex; i++)
                StartCoroutine(ColumnAddOneSquare());
        }


        if (FirstEmptySlotIndex == 0)
            return;
        transform.GetChild(FirstEmptySlotIndex - 1).gameObject.SetActive(true);
    }


    public IEnumerator SpawnFirstColumn(SquareObjPool pool)
    {
        for (int i = 0; i < 8; i++)
        {
            if (playerBornData.IsPlayerBornColumn && i == playerBornData.BornIndex)
            {
                GameObject player = Instantiate(Resources.Load<GameObject>("Prefab/PlayerSquare"), squareSpawner.position, Quaternion.identity, null);
                yield return  player.GetComponent<Square>().LooseSelf();
            }
            else
                StartCoroutine(ColumnAddOneSquare());

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// 当一个槽被填满，解锁上方的一个槽
    /// </summary>
    public void ActiveNewSlot(int _SquareIndex)
    {

        if (_SquareIndex - 1 < 0)
            return;
        transform.GetChild(_SquareIndex - 1).gameObject.SetActive(true);
    }

    /// <summary>
    /// 本列生成一个新方块
    /// </summary>
    public void AppearNewSquare(GameObject square)
    {
        square.transform.position = squareSpawner.position;
        StartCoroutine(square.GetComponent<Square>().LooseSelf());
    }

    public void LooseOneSquare() 
    {
        if(SquareNum-1>=0)
        SquareNum--;
    }

     public void GetOneSquare() 
    {
        if(SquareNum+1<=8)
        SquareNum++;

        if(SquareNum<=8)
            canCheck = true;
    }

    public IEnumerator ColumnAddOneSquare()
    {
        if (SquareNum + 1 <= 8)
        {
            GameObject newSquare = transform.parent.parent?.GetComponent<SquareGroup>().SquarePool.GetRandomSquare();
            StartCoroutine(FallNewSquare(newSquare));
            yield return null;
        }
        

    }

    IEnumerator FallNewSquare(GameObject square)
    {
        yield return new WaitForSeconds(0.2f);
        AppearNewSquare(square);
    }

}

[Serializable]
public class PlayerBornData
{
    [Header("玩家此列出生?")]
    public bool IsPlayerBornColumn;
    [Header("玩家出生位置【越靠列下方值越大】")]
    public int BornIndex;
}
