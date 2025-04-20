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

    [Header("���з�����ƶ�����")]
    public E_CustomDir  MoveDir;

    Vector3 gravityDir;
    Vector3 looseSpeed;

    SquareObjPool  squarePool;

    /// <summary>
    /// Ĭ���ɶ��ٶ�
    /// </summary>
    [Header("Ĭ���ɶ��ٶ�")]
    public float squareLooseSpeed = 50;


    void GetLoosepeed()
    {
        switch (MoveDir)
        {
            case E_CustomDir.��:
                gravityDir = Vector3.up;
                break;
            case E_CustomDir.��:
                gravityDir = Vector3.down;
                break;
            case E_CustomDir.��:
                gravityDir = Vector3.left;
                break;
            case E_CustomDir.��:
                gravityDir = Vector3.right;
                break;
            default:
                 break;
        }
        looseSpeed = gravityDir * squareLooseSpeed;
    }


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
    private float removeInterval = 0.25f;


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
        GetLoosepeed();
        squareSpawner = transform.GetChild(8);

        squarePool=FindAnyObjectByType<SquareObjPool>();

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
        for (int i = columnSlots.Count; i >= 0; i--)
        {
            if (transform.GetChild(i).childCount != 0)
            {
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
            //��Ҳ���
            if (playerBornData.IsPlayerBornColumn && i == playerBornData.BornIndex)
            {
                GameObject player = Instantiate(Resources.Load<GameObject>("Prefab/PlayerSquare"), squareSpawner.position, Quaternion.identity, null);
                 StartCoroutine(ColFallOneSquare(player));
                //ColFallOneSquare(player);

                //player.GetComponent<Square>().LooseSelf();
                yield return delay;
                continue;
            }

            Debug.Log("���ǹ��յ�");
            GameObject newSquare = squarePool.GetTargetSquare(soList[i]);
            StartCoroutine(ColFallOneSquare(newSquare));
            //ColFallOneSquare(newSquare);
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


    int currentSquareNum;
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
                //ColFull = false;
                break;
            }
            ColFull = true;
        }

        if (!ColFull)
            return;
        currentSquareNum = 0;
        for (int i = 0; i < columnSquares.Count; i++)
        {
            if (columnSquares[i])
            {
                currentSquareNum++;
            }
        }
        SquareNum = currentSquareNum;

    }
    bool canCheck = true;

    private void Update()
    {
        if (ColFull)
            RemoveSquares();

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            //���ü��
            if(!canCheck && isRemoving)
            {
                Debug.Log("�����Ƴ�");
                StopCoroutine(emptyCheck);
                timer = 0;
                canCheck = true;
            }
        }
        if (!isRemoving && SquareNum != 8 && !GetComponent<SquareRow>().isRemoving && canCheck && ColFull)
        {
            canCheck = false;
            timer = 3;//5s�ڼ��
            emptyCheck = StartCoroutine(AddWholeSquares());
        }
    }
    float timer; 


    Coroutine emptyCheck;
    IEnumerator AddWholeSquares() 
    {        
        WaitForSeconds delay=new WaitForSeconds(2f);
        yield return delay;
        for (int i = 0; i < 8 - SquareNum; i++)
        {
           ColumnAddOneRandomSquare();
           yield return new WaitForSeconds(0.8f);;
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
        }
        callback?.Invoke();
    }

    IEnumerator WaitRemoveSpawen() 
    {
        yield return new WaitForSeconds (0.8f);
            ColumnAddOneRandomSquare();

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
            ColumnAddOneRandomSquare();
            yield return new WaitForSeconds(0.02f);
        }
    }


    /// <summary>
    /// ��������һ���·���
    /// </summary>
    public void AppearNewSquare(GameObject square)
    {
        square.transform.position = squareSpawner.position;
        square.GetComponent<Square>().LooseSelf();
    }

    /// <summary>
    /// ����������һ�������ɫɫ�飬��ʼ��������
    /// </summary>
    public void ColumnAddOneRandomSquare()
    {
        GameObject newSquare = squarePool.GetRandomSquare();
        StartCoroutine(ColFallOneSquare(newSquare));
        //ColFallOneSquare(newSquare);
    }

    //�������·��������
    SquareController  newSquareController;

    /// <summary>
    /// ��һ�������ʼ������׹
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    IEnumerator ColFallOneSquare(GameObject square)
    {
        //yield return new WaitForSeconds(0.1f);
        yield return null;
        newSquareController = square.GetComponent<SquareController>();
        newSquareController.InitSquare(squareSpawner.position,gravityDir,looseSpeed);
        newSquareController.SquareLoose();
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
