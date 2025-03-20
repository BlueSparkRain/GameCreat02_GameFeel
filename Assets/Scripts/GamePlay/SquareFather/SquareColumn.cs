using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
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

    /// <summary>
    /// ���������ɷ�������ϲ�۵���������
    /// </summary>
    public int FirstEmptySlotIndex;

    public bool ColFull;

    public bool isRemoving;

    /// <summary>
    /// ������������
    /// </summary>
    private float removeInterval = 0.1f;


    public int maxSpawnNum = 8;

    public void AddMaxSpawnNum() 
    {
        maxSpawnNum++;
    }

    private void Awake()
    {

        SquareIndex = 7;
        for (int i = 0; i < 8; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(SquareIndex).gameObject.SetActive(true);

        squareSpawner = transform.GetChild(8);
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


    public IEnumerator SpawnFirstColumn(SquareObjPool pool)
    {
        for (int i = 0; i < maxSpawnNum; i++)
        {
            if (playerBornData.IsPlayerBornColumn && i == playerBornData.BornIndex)
            {
                GameObject player = Instantiate(Resources.Load<GameObject>("Prefab/PlayerSquare"), squareSpawner.position, Quaternion.identity, null);
                yield return player.GetComponent<Square>().LooseSelf();
            }
            else
                StartCoroutine(ColumnAddOneSquare());

            yield return new WaitForSeconds(removeInterval);
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
    bool canCheck=true;

    IEnumerator CanColCheck() 
    {
        if (canColCheck && !canCheck)
        {
            canColCheck = false;
            yield return new WaitForSeconds(0.4f);
            //if (FirstEmptySlotIndex != 0 || SquareNum != 8)
            //if (FirstEmptySlotIndex != 0  && !isRemoving && !GetComponent<SquareRow>().isRemoving)
            if (FirstEmptySlotIndex != 0  && !isRemoving)
            {
                yield return new WaitForSeconds(0.4f);

                if (FirstEmptySlotIndex != 0 && !isRemoving)
                {
                    ColFull = false;
                    StartCoroutine(ColumnAddOneSquare());
                    yield return new WaitForSeconds(0.1f);
                    canColCheck = true;
                    canCheck = true;
                    yield break;
                
                }
             canColCheck = true;
             canCheck = true;
             yield break;
            }
            canColCheck = true;
            canCheck = true;
            //timer = 1f;
            //yield break;
            //}
            //canColCheck=true;
        }
    }
    private void Update()
    {
        if (ColFull)
           RemoveSquares();


        //if (!isRemoving && canColCheck && ColFull && FirstEmptySlotIndex != 0 && SquareNum != 8)
        if (!isRemoving && !GetComponent<SquareRow>().isRemoving && canCheck && ColFull)
        {
            if (FirstEmptySlotIndex != 0 || SquareNum != 8)
            {
                canCheck = false;
                //canColCheck = true;
                Debug.Log("��ֵQ��");
                StartCoroutine(CanColCheck());
            }
        }

        //if (timer >= 0)
        //    timer -= Time.deltaTime;
        //else
        //{
        //    canColCheck = true;
        //}
    }

    bool canColCheck=true;
    float timer;


    public void RemoveSquares()
    {
        if (CheckRemoveList() != null)
            StartCoroutine(CheckAndRemoveSquares(CheckRemoveList()));
    }
    public List<Square> CheckRemoveList()
    {
        int num = 1;
        bool canAdd = true;

        List<Square> toRemoveSquares = new List<Square>();
        if (GetComponent<SquareRow>().isRemoving || isRemoving || !columnSquares[0] || !columnSquares[0].GetComponent<ColorSquare>().myData)
            return null;
        E_Color firstCor = columnSquares[0].GetComponent<ColorSquare>().myData.E_Color;
        toRemoveSquares.Add(columnSquares[0]);

        for (int i = 1; i < columnSquares.Count; i++)
        {
            if (!columnSquares[i] || !columnSquares[i].GetComponent<ColorSquare>())
            {
                continue;
            }

            if(!columnSquares[i].GetComponent<ColorSquare>().myData || !columnSquares[i])
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
        if (!GetComponent<SquareRow>().isRemoving &&  !isRemoving)
        {
            IsColumnRemoving();

            //yield return new WaitForSeconds(2);

            if (removeLists.Count <= 2)
            {
            }
            else if (removeLists.Count >= 5)
            {
                yield return RemoveColLine5(removeLists);
            }
            else if (removeLists.Count >= 4)
            {
                yield return RemoveColLine4(removeLists);
            }

            else if (removeLists.Count >= 3)
            {
                yield return RemoveColLine3(removeLists);
            }
            StopColumnRemoving();
        }
    }

    /// <summary>
    /// ����3�����޹��ܣ�ֻ����
    /// </summary>
    public IEnumerator RemoveColLine3(List<Square> toRemoveSquares)
    {
        Debug.Log("���3��");
        yield return RemoveLine(toRemoveSquares);
 
    }

    /// <summary>
    /// ����4��������+����1������ը��ɫ��
    /// </summary>
    public IEnumerator RemoveColLine4(List<Square> toRemoveSquares)
    {
        Debug.Log("���4��");
        yield return RemoveLine(toRemoveSquares);
    
    }

     IEnumerator RemoveLine(List<Square> toRemoveSquares) 
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
            yield return toRemoveSquares[i].BeRemoved();
            yield return ColumnAddOneSquare();
            yield return new WaitForSeconds(removeInterval);
        }
    }

    /// <summary>
    /// ����5��������+����ɫ�����磺���������ͬ��ɫɫ��
    /// </summary>
    public IEnumerator RemoveColLine5(List<Square> toRemoveSquares)
    {
        Debug.Log("���5��");
        yield return RemoveLine(toRemoveSquares);
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

   
    /// <summary>
    /// �������ϲ�Ŀղ�����
    /// </summary>
    /// <param name="index"></param>
    public void UpdateTopSlot(int index)
    {
        //if (FirstEmptySlotIndex < index)
            //return;
        GetOneSquare();
        FirstEmptySlotIndex = index;
        StartCoroutine(CheckSlotEmpty());///

        if (FirstEmptySlotIndex == 0)
            return;
        transform.GetChild(FirstEmptySlotIndex - 1).gameObject.SetActive(true);
        Debug.Log("hahh");
    }

    public IEnumerator CheckSlotEmpty() 
    {

        yield break;

        if (!isRemoving && !GetComponent<SquareRow>().isRemoving && ColFull )
        {
            if (FirstEmptySlotIndex != 0)
            {
                int toBorn = FirstEmptySlotIndex;
                for (int i = 0; i < toBorn; i++)
                {
                    if (ColFull && transform.GetChild(i).childCount == 0)
                    {
                        StartCoroutine(ColumnAddOneSquare());
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
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
        if (SquareNum + 1 <= 8 ||FirstEmptySlotIndex >0)
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
