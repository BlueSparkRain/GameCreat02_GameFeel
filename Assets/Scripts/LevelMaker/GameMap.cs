using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    //行与列数相等
    [Header("方块列集合-第一列在最左侧")]
    public List<GameCol> Columns = new List<GameCol>();
    [Header("方块行集合-第一行在最下方")]
    public List<GameRow> Rows = new List<GameRow>();

    [Header("关卡中最大行")]
    public int InitRowNum;

    Transform targetCol;
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            targetCol = transform.GetChild(i);
            Columns.Add(targetCol.GetComponent<GameCol>());
            Rows.Add(targetCol.GetComponent<GameRow>());
        }

        for (int i = 0; i < InitRowNum; ++i)
        {
            Rows[i].InitRow(Columns.Count);
        }
    }

    IEnumerator TestMovePoswer() 
    {
        yield return new WaitForSeconds(3);
        Debug.Log("变身");
        //Columns[3].mapSlots[6].GetComponentInChildren<SquareController>().GetMovePower();
    }
   
    void Start()
    {
        InitMapSlotType();

        StartCoroutine(LoadInitSquarePan());

        StartCoroutine(TestMovePoswer());
    }

    IEnumerator LoadInitSquarePan()
    {
        yield return new WaitForSeconds(1);
        LoadWholePan();
    }


    /// <summary>
    /// 根据随机地图装填一列方块
    /// </summary>
    /// <param name="colIndex"></param>
    /// <param name="soLists"></param>
    void FirstColSquares(int colIndex, List<ColorSquareSO> soLists)
    {
        //修改
        Columns[colIndex].CallSubColsFirstFull(soLists);
    }

    void BornAllSquares(int W)
    {
        int[,] validArray = RandMapGenerator.GetRandomArray(W, W);

        for (int i = 0; i < W; i++)
        {
            List<int> intList = new List<int>();

            for (int j = 0; j < W; j++)
            {
                intList.Add(validArray[i, j]);
            }

            List<ColorSquareSO> soList = FindAnyObjectByType<SquarePoolManager>().GetColorSOList(intList);

            ///测试
            //string strs="列："+i.ToString()+":";
            //for (int j = 0; j < intList.Count; j++)
            //{
            //    strs+=intList[j].ToString()+soList[j].E_Color+",";
            //}
            //Debug.Log(strs);
            ///
            FirstColSquares(i, soList);
        }

    }

    /// <summary>
    /// 充满棋盘格
    /// </summary>
    void LoadWholePan()
    {
        //以列数生成矩形方阵
        BornAllSquares(Columns.Count);

    }

    public E_AStarNodeType[,] AstarTypeMap;

    /// <summary>
    /// 初始化可以寻路的网格槽类型
    /// </summary>
    void InitMapSlotType()
    {
        E_SlotType slotType = E_SlotType.spawnerSlot;
        GameCol col;
        AstarTypeMap = new E_AStarNodeType[Columns.Count, InitRowNum];


        for (int i = 0; i < Columns.Count; i++)
        {
            col = Columns[i];
            for (int j = 0; j < col.mapSlots.Count; j++)
            {
                slotType = col.mapSlots[j].type;

                if (slotType == E_SlotType.walkableSlot)
                    AstarTypeMap[i, j] = E_AStarNodeType.walkable;
                else
                    AstarTypeMap[i, j] = E_AStarNodeType.obstacable;
                //Debug.Log(AstarTypeMap[i, j]+"哈哈");
            }
        }
        //AStarManager同步更新节点地图
        AStarManager.Instance.InitMap(Columns.Count, InitRowNum, AstarTypeMap);
    }

    /// <summary>
    /// 更新发生变动的行
    /// </summary>
    /// <param name="square"></param>
    /// <param name="ColIndex"></param>
    /// <param name="index"></param>
    public void UpdateRowSquares(Square square, int ColIndex, int index)
    {
        Rows[index - 1].UpdateRowSquare(square, ColIndex);// rowSquares[ColIndex] = square;
    }

#if UNITY_EDITOR


#endif

}
