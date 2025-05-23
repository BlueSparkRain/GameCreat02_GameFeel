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
    #region 脚本菜单拓展

    [Header("生成槽数")]
    [SerializeField] private int createSlotNum;

    [Header("最新槽")]
    Transform firstSlot;

    public GameObject WalkableSlotPrefab;
    public GameObject SpawnerSlotPrefab;
    public GameObject ObstacleSlotPrefab;

    public float slotLineWidth = 1;

#if UNITY_EDITOR
    [ContextMenu("为本列创建若干Walkable槽")]
    void CreatWalkableCol()
    {
        Vector3 firstSlotPos = firstSlot.position;
        for (int i = 0; i < createSlotNum; i++)
        {
            //纵向排列，相邻槽间隔距离为2
            GameObject newSlot = Instantiate(WalkableSlotPrefab, firstSlotPos + new Vector3(0, -slotLineWidth, 0) * (i + 1), Quaternion.identity, transform);
            if (i == createSlotNum - 1)
                firstSlot = newSlot.transform;
        }
    }

    [ContextMenu("为本列创建若干Obstacle槽")]
    void CreateObstacleCol()
    {
        Vector3 firstSlotPos = firstSlot.position;
        for (int i = 0; i < createSlotNum; i++)
        {
            //纵向排列，相邻槽间隔距离为2
            GameObject newSlot = Instantiate(ObstacleSlotPrefab, firstSlotPos + new Vector3(0, -slotLineWidth, 0) * (i + 1), Quaternion.identity, transform);
            if (i == createSlotNum - 1)
                firstSlot = newSlot.transform;
        }
    }

    [ContextMenu("为本列创建一个Spawner槽")]
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

    [Header("玩家出生信息")]
    public PlayerBornData playerBornData;

    [Header("本列方块的移动方向")]
    public E_CustomDir MoveDir;

    Vector3 gravityDir;
    Vector3 looseSpeed;

    /// <summary>
    /// 默认松动速度
    /// </summary>
    [Header("默认松动速度")]
    public float squareLooseSpeed = 40;

    //将用于行消除判断
    public List<Slot> mapSlots = new List<Slot>();
    //用于分配子列
    List<Slot> allSlots = new List<Slot>();

    SubCol currentSubCol;
    int subColNum;

    GameRow row;

    List<SubCol> subCols = new List<SubCol>();

    /// <summary>
    /// 通知子列充满自身
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
    //根据关卡槽来拆分soList

    bool newSub;
    List<ColorSquareSO>[] GetSubSOList(List<ColorSquareSO> soList) 
    {
        // 创建子列表的数组
        List<ColorSquareSO>[] soLists = new List<ColorSquareSO>[subColNum];
        offset = 0;


        for (int i = 0; i < subColNum; i++)
        {
            List<ColorSquareSO> subSoList = new List<ColorSquareSO>();

            //Debug.Log(transform.GetSiblingIndex() + "列 " + i + " 偏移: " + offset);
            // 遍历 mapSlots 并为每个子列表添加 walkableSlot 类型的元素

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
                    // 完成当前子列表的填充
                    soLists[i] = subSoList;
                    break;
                }
            }
          
            // 确保我们跳过所有不合法的槽，直到遇到合法的槽
            while (offset < mapSlots.Count && mapSlots[offset].type != E_SlotType.walkableSlot)
            {
                offset++;
            }
        }
        return soLists;

    }

    private void Awake()
    {
        //计算方块的下坠参数
        CalcuLooseSpeed();

        //预缓存本列所有槽对象
        for (int i = 0; i < transform.childCount; i++)
        {
            allSlots.Add(transform.GetChild(i).GetComponent<Slot>());
        }

        for (int i = 0; i < allSlots.Count; i++)
        {
            allSlots[i].HideSelf();
        }

        CreatAllSubCols();
        //为关卡槽设置寻路序列

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
    /// 计算方块的下坠参数
    /// </summary>
    void CalcuLooseSpeed()
    {
        switch (MoveDir)
        {
            case E_CustomDir.上:
                gravityDir = Vector3.up;
                break;
            case E_CustomDir.下:
                gravityDir = Vector3.down;
                break;
            case E_CustomDir.左:
                gravityDir = Vector3.left;
                break;
            case E_CustomDir.右:
                gravityDir = Vector3.right;
                break;
            default:
                break;
        }
        looseSpeed = gravityDir * squareLooseSpeed;
    }

    bool col;

    /// <summary>
    /// 创建若干子列
    /// </summary>
    void CreatAllSubCols()
    {
        subColNum = 0;
        subCols.Clear();
        //首先获取该列下所有的槽
        for (int i = 0; i <= allSlots.Count; ++i)
        {
            if (i == allSlots.Count)
            {
                if (col)
                {
                    col = false;
                    //当前子列读取完毕，执行初始化操作
                    currentSubCol.Init(gravityDir, looseSpeed);
                }
                break;
            }

            //只要不是第一个孵化器，就视为地图内槽
            if (i != 0)
                mapSlots.Add(allSlots[i].GetComponent<Slot>());


            //检测到首个孵化者，开启新子列
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
            //检测到障碍槽，
            else if (allSlots[i].type == E_SlotType.obstacleSlot)
            {
                if (col)
                {
                    col=false;
                    //当前子列读取完毕，执行初始化操作
                    currentSubCol.Init(gravityDir, looseSpeed);
                }
                //判断是否是玩家出生列
                if (playerBornData.SubColIndex == subColNum)
                    currentSubCol.IsPlayerCol(playerBornData.BornIndex);
                //说明一个子列读取完毕,接着结束或进行下一子类
                continue;
            }
            //正常的槽
            else
            {
                if (playerBornData.SubColIndex == subColNum)
                    currentSubCol.IsPlayerCol(playerBornData.BornIndex);
                //说明一个子列读取完毕,接着结束或进行下一子类
                //continue;
                //else
                //为当前子列添加新的槽
                currentSubCol.GetNewSlot(allSlots[i]);
            }
        }
    }


    /// <summary>
    /// 更新本列方块
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
    [Header("玩家于此列出生")]
    public bool IsPlayerBornColumn;
    [Header("出生所在子竖列序号（至小应为1）")]
    public int SubColIndex = 0;
    [Header("玩家在子竖列的出生位置【越靠列下方值越大】")]
    public int BornIndex;
}

