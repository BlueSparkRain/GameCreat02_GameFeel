using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SquareGroup : MonoBehaviour
{
    //ֻ�е�һ�л�һ��ȫ���������ſ�ʼ���
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
    /// ���·����䶯���еķ���
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
    /// ���Ŀ����
    /// </summary>
    /// <param name="ColIndex"></param>
    public void RemoveOneCol(int ColIndex) 
    {
        StartCoroutine(Columns[ColIndex].RemoveWholeColumn());
    }

    /// <summary>
    /// ���Ŀ����
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
    /// �������̸�
    /// </summary>
    void LoadWholePan() 
    {
        for (int i = 0; i < 8; i++)
        {
            ColumnSpawenNewSquare(i,SquarePool);
        }
    }
    /// <summary>
    /// ÿ�и��Գ���
    /// </summary>
    /// <param name="column"></param>
    /// <param name="pool"></param>
    public void ColumnSpawenNewSquare(int column,SquareObjPool pool)
    {
      StartCoroutine(Columns[column].SpawnFirstColumn(pool));
    }
}
