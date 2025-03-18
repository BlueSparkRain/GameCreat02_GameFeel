using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRow :MonoBehaviour
{
    public List<Square> rowSquares = new List<Square>();

    public int SquareIndex { get; private set; }
    [Header("���з���")]
    public int SquareNum;

    public bool RowFull;

    public bool isRemoving;
    private List<Square> toRemoveSquares = new List<Square>();

    private float spawnInterval = 0.15f;

    /// <summary>
    /// ����Ŀ����Square
    /// </summary>
    /// <param name="RowIndex"></param>
    /// <returns></returns>
    public IEnumerator  RemoveWholeRow() 
    {
        for (int i = 0; i <rowSquares.Count; i++)
        {
            if (rowSquares[i].GetComponent<PlayerController>())
            {
                if (i + 1 >= rowSquares.Count)
                    yield break;
                i++;
            }

            Transform targetCol = rowSquares[i].transform?.parent.parent;
            yield return rowSquares[i].BeRemoved();
            yield return targetCol?.GetComponent<SquareColumn>().ColumnAddOneSquare();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void UpdateRowSquareList() 
    {
        for (int i = 0;i < rowSquares.Count; i++) 
        {
            if (!rowSquares[i])
            {
                RowFull = false;
                break;
            }
            RowFull = true;
        }

        if (!RowFull)
            return;
        CheckRowCanRemove();
    }

    public void CheckRowCanRemove()
    {
       
        if (GetComponent<SquareColumn>().isRemoving||!rowSquares[rowSquares.Count-1] || isRemoving)
            return;
        int num = 1;
        bool canAdd = true;

        toRemoveSquares.Clear();

        E_Color firstCor = rowSquares[0].GetComponent<ColorSquare>().myData.E_Color;
        toRemoveSquares.Add(rowSquares[0]);


        for (int i = 1; i <rowSquares.Count; i++)
        {
            if (rowSquares[i]!=null && rowSquares[i].GetComponent<ColorSquare>() && rowSquares[i].GetComponent<ColorSquare>().myData.E_Color == firstCor && canAdd)
            {
                    if (!toRemoveSquares.Contains(rowSquares[i]))
                    {
                        num++;
                        toRemoveSquares.Add(rowSquares[i]);
                    }
        
                //Debug.Log(firstCor + "ͬɫ" + num);
            }
            else
            {
                if (num < 3)
                {
                    firstCor = rowSquares[i].GetComponent<ColorSquare>().myData.E_Color;
                    toRemoveSquares.Clear();
                    //Debug.Log("��ɫ-��ɫ" + firstCor);
                    num = 1;
                    canAdd = true;
                    toRemoveSquares.Add(rowSquares[i]);
                }
                else
                {
                    canAdd = false;
                    toRemoveSquares.Remove(rowSquares[i]);
                }
            }
        }
        //Debug.Log("�������:" + firstCor.ToString() + num);
        //���������У�ִ���������

        StartCoroutine(RemoveSquares(num));
    }

    IEnumerator RemoveSquares(int num)
    {
        //if (!RowFull)
        //    yield break;

        if (!isRemoving )
        {

            isRemoving = true;
            if (num <= 2)
            {

            }
            else if (num >= 5)
            {
                yield return RemoveColLine5();
            }
            else if (num >= 4)
            {
                yield return RemoveColLine4();
            }

            else if (num >= 3)
            {
                yield return RemoveColLine3();
            }
            //yield return new WaitForSeconds(0.2f);
            //transform.parent.parent.GetComponent<SquareGroup>().isRemoving = false;
            isRemoving = false;
        }
    }

    /// <summary>
    /// ����3�����޹��ܣ�ֻ����
    /// </summary>
    public IEnumerator RemoveColLine3()
    {
        Debug.Log("���3��");


        for (int i = 0; i < toRemoveSquares .Count; i++)
        {
            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                i++;
            }

            if (toRemoveSquares[i].transform.parent!= null)
            {
                Transform targetCol = toRemoveSquares[i].transform?.parent.parent;
                yield return toRemoveSquares[i].BeRemoved();
                yield return targetCol?.GetComponent<SquareColumn>().ColumnAddOneSquare();
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    /// <summary>
    /// ����4��������+����1������ը��ɫ��
    /// </summary>
    public IEnumerator RemoveColLine4()
    {
        for (int i = 0; i < toRemoveSquares.Count; i++)
        {
            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                i++;
            }
            if (toRemoveSquares[i].transform.parent != null)
            {
                Transform targetCol = toRemoveSquares[i].transform?.parent.parent;
                yield return toRemoveSquares[i].BeRemoved();
                yield return targetCol?.GetComponent<SquareColumn>().ColumnAddOneSquare();
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    /// <summary>
    /// ����5��������+����ɫ�����磺���������ͬ��ɫɫ��
    /// </summary>
    public IEnumerator RemoveColLine5()
    {
        Debug.Log("���5��");
        for (int i = 0; i < toRemoveSquares.Count; i++)
        {
            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                i++;
            }

            if (toRemoveSquares[i].transform.parent != null)
            {
                Transform targetCol = toRemoveSquares[i].transform?.parent.parent;
                yield return toRemoveSquares[i].BeRemoved();
                yield return targetCol?.GetComponent<SquareColumn>().ColumnAddOneSquare();
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }


}
