using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    //�����������
    [Header("�����м���-��һ���������")]
    public List<GameCol> Columns = new List<GameCol>();
    [Header("�����м���-��һ�������·�")]
    public List<GameRow> Rows = new List<GameRow>();

    [Header("�ؿ��������")]
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
        //Debug.Log("����");
        Columns[3].mapSlots[6].GetComponentInChildren<SquareController>().GetMovePower();
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

    public void FirstColSquares(int colIndex, List<ColorSquareSO> soLists)
    {
        //�޸�
        Columns[colIndex].CallSubColsFirstFull(soLists);
    }

    /// <summary>
    /// �������̸�
    /// </summary>
    void LoadWholePan()
    {
        //���������ɾ��η���
        ColorSquareManager.Instance.BornAllSquares(Columns.Count);
    }

    public E_AStarNodeType[,] AstarTypeMap;

    /// <summary>
    /// ��ʼ������Ѱ·�����������
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
                //Debug.Log(AstarTypeMap[i, j]+"����");
            }
        }
        //AStarManagerͬ�����½ڵ��ͼ
        AStarManager.Instance.InitMap(Columns.Count, InitRowNum, AstarTypeMap);
    }


    /// <summary>
    /// ���·����䶯����
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
