using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SquareGroup : MonoBehaviour
{
    [Header("�����м���-��һ���������")]
    public List<SquareColumn> Columns=new List<SquareColumn>();
    [Header("�����м���-��һ�������·�")]
    public List<SquareRow> Rows = new List<SquareRow>();

    [Header("����Obj��")]
    public SquareObjPool SquarePool;

    [Header("Ҫ��������")]
    public int RemoveCol;
    [Header("Ҫ��������")]
    public int RemoveRow;

    public bool isRemoving;
    
    void Start()
    {
        StartCurrentGame();
    }

    /// <summary>
    /// ��Ϸ��ʼ�����س�ʼ8*8��������
    /// </summary>
    void StartCurrentGame() 
    {
        for (int i = 0; i < 8; i++)
        {
            Columns.Add(transform.GetChild(0).GetChild(i).GetComponent<SquareColumn>());
        }

        StartCoroutine(LoadInitSquarePan());

        for (int i = 0; i < 8; i++)
        {
            Rows.Add(transform.GetChild(0).GetChild(i).GetComponent<SquareRow>());
        }
    }
    IEnumerator LoadInitSquarePan()
    {
        yield return new WaitForSeconds(1);
        LoadWholePan();
    }

    /// <summary>
    /// �������̸�
    /// </summary>
    void LoadWholePan()
    {
        for (int i = 0; i < 8; i++)
        {
            ColumnSpawenNewSquare(i, SquarePool);
        }
    }

    /// <summary>
    /// ֪ͨÿ�и��Գ���
    /// </summary>
    /// <param name="column"></param>
    /// <param name="pool"></param>
    public void ColumnSpawenNewSquare(int column, SquareObjPool pool)
    {
      StartCoroutine(Columns[column].SpawnFirstColumn(pool));
    }

    /// <summary>
    /// ���·����䶯����
    /// </summary>
    /// <param name="square"></param>
    /// <param name="ColIndex"></param>
    /// <param name="index"></param>
    public void UpdateRowSquares(Square square,int ColIndex, int index)
    {
        if (Rows.Count < 8)
            return;
        Rows[index].rowSquares[ColIndex] =square;
        Rows[index].UpdateRowFullState();
    }


    /// <summary>
    /// ���Ŀ��ȫ��
    /// </summary>
    /// <param name="ColIndex"></param>
    public void RemoveOneCol(int ColIndex) 
    {
        StartCoroutine(Columns[ColIndex].RemoveWholeColumn());
    }

    /// <summary>
    /// ���Ŀ��ȫ��
    /// </summary>
    /// <param name="rowIndex"></param>
    public void RemoveOneRow(int rowIndex) 
    {
        StartCoroutine(Rows[rowIndex].RemoveWholeRow());
    }
}
