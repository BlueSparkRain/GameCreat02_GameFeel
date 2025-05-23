using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameCol : MonoBehaviour
{
    #region �ű��˵���չ

    [Header("���ɲ���")]
    [SerializeField] private int createSlotNum;

    [Header("���²�")]
    Transform firstSlot;

    public GameObject WalkableSlotPrefab;
    public GameObject SpawnerSlotPrefab;
    public GameObject ObstacleSlotPrefab;

    public float slotLineWidth = 1;

#if UNITY_EDITOR
    [ContextMenu("Ϊ���д�������Walkable��")]
    void CreatWalkableCol()
    {
        Vector3 firstSlotPos = firstSlot.position;
        for (int i = 0; i < createSlotNum; i++)
        {
            //�������У����ڲۼ������Ϊ2
            GameObject newSlot = Instantiate(WalkableSlotPrefab, firstSlotPos + new Vector3(0, -slotLineWidth, 0) * (i + 1), Quaternion.identity, transform);
            if (i == createSlotNum - 1)
                firstSlot = newSlot.transform;
        }
    }

    [ContextMenu("Ϊ���д�������Obstacle��")]
    void CreateObstacleCol()
    {
        Vector3 firstSlotPos = firstSlot.position;
        for (int i = 0; i < createSlotNum; i++)
        {
            //�������У����ڲۼ������Ϊ2
            GameObject newSlot = Instantiate(ObstacleSlotPrefab, firstSlotPos + new Vector3(0, -slotLineWidth, 0) * (i + 1), Quaternion.identity, transform);
            if (i == createSlotNum - 1)
                firstSlot = newSlot.transform;
        }
    }

    [ContextMenu("Ϊ���д���һ��Spawner��")]
    void CreatSpawnerCol()
    {
        Vector3 firstSlotPos;

        if (firstSlot != null)
            firstSlotPos = firstSlot.position;
        else
            firstSlotPos = transform.position;

        GameObject newSlot = Instantiate(SpawnerSlotPrefab, firstSlotPos + new Vector3(0, -slotLineWidth, 0), Quaternion.identity, transform);
        firstSlot = newSlot.transform;
    }
#endif

    #endregion

    [Header("��ҳ�����Ϣ")]
    public PlayerBornData playerBornData;

    [Header("���з�����ƶ�����")]
    public E_CustomDir MoveDir;

    Vector3 gravityDir;
    Vector3 looseSpeed;

    /// <summary>
    /// Ĭ���ɶ��ٶ�
    /// </summary>
    [Header("Ĭ���ɶ��ٶ�")]
    public float squareLooseSpeed = 40;

    //�������������ж�
    public List<Slot> mapSlots = new List<Slot>();
    //���ڷ�������
    List<Slot> allSlots = new List<Slot>();

    SubCol currentSubCol;
    int subColNum;

    GameRow row;

    List<SubCol> subCols = new List<SubCol>();

    /// <summary>
    /// ֪ͨ���г�������
    /// </summary>
    public void CallSubColsFirstFull(List<ColorSquareSO> soList) 
    {
        var soLists = GetSubSOList(soList);
        for (int i= subCols.Count-1; i >=0; i--)
        {
            StartCoroutine(subCols[i].SpawneFirstColSquares(soLists[i]));
        }
    }

    int offset;
    //���ݹؿ��������soList

    bool newSub;
    List<ColorSquareSO>[] GetSubSOList(List<ColorSquareSO> soList) 
    {
        // �������б������
        List<ColorSquareSO>[] soLists = new List<ColorSquareSO>[subColNum];
        offset = 0;


        for (int i = 0; i < subColNum; i++)
        {
            List<ColorSquareSO> subSoList = new List<ColorSquareSO>();

            //Debug.Log(transform.GetSiblingIndex() + "�� " + i + " ƫ��: " + offset);
            // ���� mapSlots ��Ϊÿ�����б���� walkableSlot ���͵�Ԫ��

            for (int j = offset; j <= mapSlots.Count; j++) 
            {
                if (offset == mapSlots.Count)
                {
                    soLists[i] = subSoList;

                    break;
                }

                if (mapSlots[offset].type == E_SlotType.walkableSlot)
                {
                    subSoList.Add(soList[offset]);
                    offset++;
                }
                else
                {
                    // ��ɵ�ǰ���б�����
                    soLists[i] = subSoList;
                    break;
                }
            }
          
            // ȷ�������������в��Ϸ��Ĳۣ�ֱ�������Ϸ��Ĳ�
            while (offset < mapSlots.Count && mapSlots[offset].type != E_SlotType.walkableSlot)
            {
                offset++;
            }
        }
        return soLists;

    }

    private void Awake()
    {
        //���㷽�����׹����
        CalcuLooseSpeed();

        //Ԥ���汾�����в۶���
        for (int i = 0; i < transform.childCount; i++)
        {
            allSlots.Add(transform.GetChild(i).GetComponent<Slot>());
        }

        for (int i = 0; i < allSlots.Count; i++)
        {
            allSlots[i].HideSelf();
        }

        CreatAllSubCols();
        //Ϊ�ؿ�������Ѱ·����

        for (int i = mapSlots.Count-1; i >=0; --i)
        {
            mapSlots[i].GetPathFindIndex(new Vector2Int(transform.GetSiblingIndex(), i));
        }

        row=GetComponent<GameRow>();
        
        for (int i = 0;i < allSlots.Count; i++) 
        {
            allSlots[i].mapIndex = i;
        }
    }

    /// <summary>
    /// ���㷽�����׹����
    /// </summary>
    void CalcuLooseSpeed()
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

    bool col;

    /// <summary>
    /// ������������
    /// </summary>
    void CreatAllSubCols()
    {
        subColNum = 0;
        subCols.Clear();
        //���Ȼ�ȡ���������еĲ�
        for (int i = 0; i <= allSlots.Count; ++i)
        {
            if (i == allSlots.Count)
            {
                if (col)
                {
                    col = false;
                    //��ǰ���ж�ȡ��ϣ�ִ�г�ʼ������
                    currentSubCol.Init(gravityDir, looseSpeed);
                }
                break;
            }

            //ֻҪ���ǵ�һ��������������Ϊ��ͼ�ڲ�
            if (i != 0)
                mapSlots.Add(allSlots[i].GetComponent<Slot>());


            //��⵽�׸������ߣ�����������
            if (allSlots[i].type == E_SlotType.spawnerSlot)
            {
                col = true;
                subColNum++;
                var newSubCol = new GameObject("SubCol:" + subColNum).AddComponent<SubCol>();
                subCols.Add(newSubCol);
                newSubCol.transform.SetParent(transform);
                newSubCol.transform.localScale = Vector3.one;



                newSubCol.GetNewSlot(allSlots[i]);

                currentSubCol = newSubCol;
            }
            //��⵽�ϰ��ۣ�
            else if (allSlots[i].type == E_SlotType.obstacleSlot)
            {
                if (col)
                {
                    col=false;
                    //��ǰ���ж�ȡ��ϣ�ִ�г�ʼ������
                    currentSubCol.Init(gravityDir, looseSpeed);
                }
                //�ж��Ƿ�����ҳ�����
                if (playerBornData.SubColIndex == subColNum)
                    currentSubCol.IsPlayerCol(playerBornData.BornIndex);
                //˵��һ�����ж�ȡ���,���Ž����������һ����
                continue;
            }
            //�����Ĳ�
            else
            {
                if (playerBornData.SubColIndex == subColNum)
                    currentSubCol.IsPlayerCol(playerBornData.BornIndex);
                //˵��һ�����ж�ȡ���,���Ž����������һ����
                //continue;
                //else
                //Ϊ��ǰ��������µĲ�
                currentSubCol.GetNewSlot(allSlots[i]);
            }
        }
    }


    /// <summary>
    /// ���±��з���
    /// </summary>
    /// <param name="square"></param>
    /// <param name="index"></param>
    public void UpdateColumnSquares(Square square, int index)
    {
       
    }
}

[Serializable]
public class PlayerBornData
{
    [Header("����ڴ��г���")]
    public bool IsPlayerBornColumn;
    [Header("����������������ţ���СӦΪ1��")]
    public int SubColIndex = 0;
    [Header("����������еĳ���λ�á�Խ�����·�ֵԽ��")]
    public int BornIndex;
}

