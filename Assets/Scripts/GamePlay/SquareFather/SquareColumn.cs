using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public List<Slot> columnSlots = new List<Slot>();

    /// <summary>
    /// ���������ɷ�������ϲ�۵���������
    /// </summary>
    public int FirstEmptySlotIndex;

    public bool ColFull;

    [Header("��������������")]
    public bool isRemoving;

    /// <summary>
    /// ������������
    /// </summary>
    private float removeInterval = 0.3f;


    [Header("����ɫ�鲹���������")]
    public int maxSpawnNum = 8;

    /// <summary>
    /// ������������ǰ���������ϵ�ɫ�飬��Ӱ�챾�еĿ�����ɫ��������������������ﱻ�������ָ���������
    /// </summary>
    public void AddMaxSpawnNum()
    {
        maxSpawnNum++;
    }

    private void Awake()
    {
        squareSpawner = transform.GetChild(8);

        //Ԥ����۶��������Ż�
        for (int i = 0; i < 8; i++)
        {
          columnSlots.Add(transform.GetChild(i).GetComponent<Slot>());
        }


        for (int i = 0; i <columnSlots.Count ; i++)
        {
            columnSlots[i].isDownEmpty = true;
        }
        columnSlots[7].isDownEmpty=false;
   
    }

    void Start()
    {
        for (int i = 7; i >= 0; i--)
        {
            if (transform.GetChild(i).childCount != 0)
            {
                UpdateTopSlot(i);
                maxSpawnNum--;
            }
        }
    }

    /// <summary>
    /// �����ɾ��ϣ���������
    /// </summary>
    /// <param name="soList"></param>
    /// <returns></returns>
    public IEnumerator SpawneFirstColSquares(List<ColorSquareSO> soList) 
    {
        WaitForSeconds delay=  new WaitForSeconds(0.2f);
        for (int i = 0; i < maxSpawnNum; i++)
        {

            if (playerBornData.IsPlayerBornColumn && i == playerBornData.BornIndex)
            {
                GameObject player = Instantiate(Resources.Load<GameObject>("Prefab/PlayerSquare"), squareSpawner.position, Quaternion.identity, null);
                player.GetComponent<Square>().LooseSelf();
                yield return delay;
                continue;
            }


            GameObject newSquare = transform.parent.parent?.GetComponent<SquareGroup>().SquarePool.GetTargetSquare(soList[i]);
            StartCoroutine(FallNewSquare(newSquare));
            yield return delay;
        }

    }

    /// <summary>
    /// ���п�����������
    /// </summary>
    public void IsColumnRemoving()
    {
        isRemoving = true;
    }

    /// <summary>
    /// ���������������
    /// </summary>
    public void StopColumnRemoving()
    {
        isRemoving = false;
    }

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
    }
    bool canCheck = true;

    private void Update()
    {
        if (ColFull)
            RemoveSquares();

        //����
        if (!isRemoving && !GetComponent<SquareRow>().isRemoving && canCheck && ColFull)
        {
            if (SquareNum != 8 && !isRemoving)
            {
                canCheck = false;
                StartCoroutine(AddWholeSquares());
            }
        }
    }

    IEnumerator AddWholeSquares() 
    {
        WaitForSeconds delay=new WaitForSeconds(0.8f);
        yield return delay;
        yield return new WaitForSeconds(2);
        for (int i = 0; i <8-SquareNum ; i++)
        {
            yield return delay;
            if (SquareNum < 8)
                ColumnAddOneSquare();
            else
                break;
        }
        canCheck = true;
    }

    public void RemoveSquares()
    {
        if (CheckRemoveList() != null)
        {
            StartCoroutine(CheckAndRemoveSquares(CheckRemoveList()));
        }
    }
    public List<Square> CheckRemoveList()
    {
        int num = 1;
        bool canAdd = true;

        List<Square> toRemoveSquares = new List<Square>();
        if (!columnSquares[0] || !columnSquares[0].GetComponent<ColorSquare>() ||!columnSquares[0].GetComponent<ColorSquare>().myData)
            return null;
        E_Color firstCor = columnSquares[0].GetComponent<ColorSquare>().myData.E_Color;
        toRemoveSquares.Add(columnSquares[0]);

        for (int i = 1; i < columnSquares.Count; i++)
        {
            if (!columnSquares[i] || !columnSquares[i].GetComponent<ColorSquare>())
            {
                continue;
            }
            if (!columnSquares[i].GetComponent<ColorSquare>().myData || !columnSquares[i])
                return null;

            if (columnSquares[i] && columnSquares[i].GetComponent<ColorSquare>() && columnSquares[i].GetComponent<ColorSquare>().myData.E_Color == firstCor && canAdd)
            {
                if (!toRemoveSquares.Contains(columnSquares[i]))
                {
                    num++;
                    toRemoveSquares.Add(columnSquares[i]);
                }
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
        if (toRemoveSquares.Count >= 3)
            return toRemoveSquares;
        else
            return null;
    }

    IEnumerator CheckAndRemoveSquares(List<Square> removeLists)
    {
        if (!GetComponent<SquareRow>().isRemoving && !isRemoving)
        {
            IsColumnRemoving();

            if (removeLists.Count <= 2)
            {
            }
            else if (removeLists.Count >= 5)
            {
                yield return RemoveLine(removeLists,RemoveColLine5); 
            }
            else if (removeLists.Count >= 4)
            {
                yield return RemoveLine(removeLists, RemoveColLine4);
            }

            else if (removeLists.Count >= 3)
            {
                yield return RemoveLine(removeLists, RemoveColLine3);
            }
            StopColumnRemoving();
        }
    }

    /// <summary>
    /// ����3�����޹��ܣ�ֻ����
    /// </summary>
    public void RemoveColLine3()
    {
        Debug.Log("���3��");
    }

    /// <summary>
    /// ����4��������+����1������ը��ɫ��
    /// </summary>
    public void RemoveColLine4()
    {
        Debug.Log("���4��");
    }

    IEnumerator RemoveLine(List<Square> toRemoveSquares,UnityAction callback=null)
    {
        for (int i = 0; i < toRemoveSquares.Count; i++)
        {
            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                yield return toRemoveSquares[i].BeRemoved();
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                i++;
            }
            StartCoroutine(toRemoveSquares[i].BeRemoved());

            StartCoroutine(WaitRemoveSpawen());

            yield return new WaitForSeconds(removeInterval);
            //ColumnAddOneSquare();
        }

        callback?.Invoke();
    }

    IEnumerator WaitRemoveSpawen() 
    {
        yield return new WaitForSeconds (1f);
            ColumnAddOneSquare();

    }

    /// <summary>
    /// ����5��������+����ɫ�����磺���������ͬ��ɫɫ��
    /// </summary>
    public void RemoveColLine5()
    {
        Debug.Log("���5��");
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
            ColumnAddOneSquare();
            yield return new WaitForSeconds(0.02f);
        }
    }


    /// <summary>
    /// �������ϲ�Ŀղ�����
    /// </summary>
    /// <param name="index"></param>
    public void UpdateTopSlot(int index)
    {
        GetOneSquare();

        FirstEmptySlotIndex = index;

        if (ColFull)
        {
            for (int i = 0; i < columnSquares.Count; i++)
            {
                if (transform.GetChild(i).childCount != 0)
                {
                    FirstEmptySlotIndex = i;
                    SquareNum = 8 - FirstEmptySlotIndex;
                    break;
                }
            }
        }


        if (FirstEmptySlotIndex == 0)
            return;
        transform.GetChild(FirstEmptySlotIndex - 1).gameObject.SetActive(true);
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
        square.GetComponent<Square>().LooseSelf();
    }

    public void LooseOneSquare()
    {
        if (SquareNum - 1 >= 0)
            SquareNum--;

        FirstEmptySlotIndex = 8 - SquareNum;
    }

    public void GetOneSquare()
    {
        if (SquareNum + 1 <= 8)
            SquareNum++;

        FirstEmptySlotIndex = 8 - SquareNum;

        if (SquareNum <= 8)
            canCheck = true;
    }

    //public IEnumerator ColumnAddOneSquare()
    public void ColumnAddOneSquare()
    {

        GameObject newSquare = transform.parent.parent?.GetComponent<SquareGroup>().SquarePool.GetRandomSquare();
        StartCoroutine(FallNewSquare(newSquare));

    }

    IEnumerator FallNewSquare(GameObject square)
    {
        yield return new WaitForSeconds(0.1f);
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
