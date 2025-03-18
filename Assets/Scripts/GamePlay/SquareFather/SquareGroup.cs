using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SquareGroup : MonoBehaviour
{
    //只有等一行或一列全部充满，才开始检测
    [Header("方块列集合-第一列在最左侧")]
    public List<SquareColumn> Columns=new List<SquareColumn>();
    [Header("方块行集合-第一行在最下方")]
    public List<SquareRow> Rows = new List<SquareRow>();

    [Header("方块Obj池")]
    public SquareObjPool SquarePool;

    [Header("要消除的列")]
    public int RemoveCol;
    [Header("要消除的行")]
    public int RemoveRow;

    public bool isRemoving;
    
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            Columns.Add(transform.GetChild(0).GetChild(i).GetComponent<SquareColumn>());
        }

        StartCoroutine(Load());

        for (int i = 0;i < 8; i++) 
        {
            Rows.Add(transform.GetChild(0).GetChild(i).GetComponent<SquareRow>());
        }
    }

    /// <summary>
    /// 更新发生变动的行的方块
    /// </summary>
    /// <param name="square"></param>
    /// <param name="ColIndex"></param>
    /// <param name="index"></param>
    public void UpdateRowSquares(Square square,int ColIndex, int index)
    {
        Rows[index].rowSquares[ColIndex] =square;
        Rows[index].UpdateRowSquareList();
    }

    void Update()
    {
      
    }

    /// <summary>
    /// 清除目标列
    /// </summary>
    /// <param name="ColIndex"></param>
    public void RemoveOneCol(int ColIndex) 
    {
        StartCoroutine(Columns[ColIndex].RemoveWholeColumn());
    }

    /// <summary>
    /// 清除目标行
    /// </summary>
    /// <param name="rowIndex"></param>
    public void RemoveOneRow(int rowIndex) 
    {
        StartCoroutine(Rows[rowIndex].RemoveWholeRow());
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(1);
        LoadWholePan();
    }

    /// <summary>
    /// 充满棋盘格
    /// </summary>
    void LoadWholePan() 
    {
        for (int i = 0; i < 8; i++)
        {
            ColumnSpawenNewSquare(i,SquarePool);
        }
    }
    /// <summary>
    /// 每列各自充满
    /// </summary>
    /// <param name="column"></param>
    /// <param name="pool"></param>
    public void ColumnSpawenNewSquare(int column,SquareObjPool pool)
    {
      StartCoroutine(Columns[column].SpawnFirstColumn(pool));
    }
}
