using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SquareColumn : MonoBehaviour
{
    [Header("������ֳ��")]
    public Transform squareSpawner;
    [Header("���ϲ㷽��λ��")]
    public int SquareIndex { get; private set; }
    [Header("���з���")]
    public int SquareNum;
    [Header("��ҳ�����Ϣ")]
    public PlayerBornData playerBornData;

    public List<Square> columnSquares = new List<Square>();
    public int FirstEmptySlotIndex;

    private List<Square> toRemoveSquares = new List<Square>();

    public bool ColFull;

    public bool isRemoving;

    private float spawnInterval = 0.15f;

    /// <summary>
    /// ���±��з���
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
               
                //Debug.Log(firstCor + "ͬɫ" + num);
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
        //Debug.Log("�������:"+ firstCor.ToString()+num);
        //���������У�ִ���������

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
    /// ����3�����޹��ܣ�ֻ����
    /// </summary>
    public IEnumerator RemoveColLine3()
    {
        Debug.Log("���3��");

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
    /// ����4��������+����1������ը��ɫ��
    /// </summary>
    public IEnumerator RemoveColLine4()
    {
        Debug.Log("���4��");
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
    /// ����5��������+����ɫ�����磺���������ͬ��ɫɫ��
    /// </summary>
    public IEnumerator RemoveColLine5()
    {
        Debug.Log("���5��");
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
    /// �������Square
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
    /// �������ϲ�Ŀղ�����
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
    /// ��һ���۱������������Ϸ���һ����
    /// </summary>
    public void ActiveNewSlot(int _SquareIndex)
    {

        if (_SquareIndex - 1 < 0)
            return;
        transform.GetChild(_SquareIndex - 1).gameObject.SetActive(true);
    }

    /// <summary>
    /// ��������һ���·���
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
    [Header("��Ҵ��г���?")]
    public bool IsPlayerBornColumn;
    [Header("��ҳ���λ�á�Խ�����·�ֵԽ��")]
    public int BornIndex;
}
