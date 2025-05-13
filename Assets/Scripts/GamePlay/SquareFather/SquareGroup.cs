using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SquareGroup : MonoBehaviour
{
    //[Header("方块列集合-第一列在最左侧")]
    //public List<SquareColumn> Columns=new List<SquareColumn>();
    //[Header("方块行集合-第一行在最下方")]
    //public List<SquareRow> Rows = new List<SquareRow>();


    //[Header("要消除的列")]
    //public int RemoveCol;
    //[Header("要消除的行")]
    //public int RemoveRow;

    //public bool isRemoving;

    //public E_AStarNodeType[,] slotType;

    //void Start()
    //{
    //    //StartCurrentGame();
    //}

    ///// <summary>
    ///// 游戏开始
    ///// </summary>
    //void StartCurrentGame() 
    //{
    //    for (int i = 0; i < 8; i++)
    //    {
    //        Columns.Add(transform.GetChild(0).GetChild(i).GetComponent<SquareColumn>());
    //    }

    //    StartCoroutine(LoadInitSquarePan());

    //    for (int i = 0; i < 8; i++)
    //    {
    //        Rows.Add(transform.GetChild(0).GetChild(i).GetComponent<SquareRow>());
    //    }
    //}
    //IEnumerator LoadInitSquarePan()
    //{
    //    yield return new WaitForSeconds(1);
    //    LoadWholePan();
    //}

    ///// <summary>
    ///// 充满棋盘格
    ///// </summary>
    //void LoadWholePan()
    //{
    //    ColorSquareManager.Instance.BornAllSquares(8);
    //}

    //public void FirstColSquares(int colIndex,List<ColorSquareSO> soLists) 
    //{
    //    //修改
    //    StartCoroutine(Columns[colIndex].SpawneFirstColSquares(soLists) );
    //}

    ///// <summary>
    ///// 更新发生变动的行
    ///// </summary>
    ///// <param name="square"></param>
    ///// <param name="ColIndex"></param>
    ///// <param name="index"></param>
    //public void UpdateRowSquares(Square square,int ColIndex, int index)
    //{
    //    if (Rows.Count < 8)
    //        return;
    //    Rows[index].rowSquares[ColIndex] =square;
    //    Rows[index].UpdateRowFullState();
    //}


    ///// <summary>
    ///// 清除目标全列
    ///// </summary>
    ///// <param name="ColIndex"></param>
    //public void RemoveOneCol(int ColIndex) 
    //{
    //    StartCoroutine(Columns[ColIndex].RemoveWholeColumn());
    //}

    ///// <summary>
    ///// 清除目标全行
    ///// </summary>
    ///// <param name="rowIndex"></param>
    //public void RemoveOneRow(int rowIndex) 
    //{
    //    StartCoroutine(Rows[rowIndex].RemoveWholeRow());
    //}
}
