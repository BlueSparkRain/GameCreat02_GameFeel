using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGroup : MonoBehaviour
{
    public List<SquareColumn>  Columns=new List<SquareColumn>();
    
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            Columns.Add(transform.GetChild(0).GetChild(i).GetComponent<SquareColumn>());
        }

        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(1);
        LoadWholePan();
    }

    /// <summary>
    /// ≥‰¬˙∆Â≈Ã∏Ò
    /// </summary>
    void LoadWholePan() 
    {
        for (int i = 0; i < 8; i++)
        {
            ColumnSpawenNewSquare(i);
        }
    }

    //public void CheckEmptySlot() 
    //{
    //    for (int i = 0; i < 8; i++) 
    //    {
    //        if (Columns[i].SquareNum < 8) 
    //        {
    //            StartCoroutine(Columns[i].SpawnWholeColumn());
    //        }
    //    }
    //}

    public void ColumnSpawenNewSquare(int column)
    {
      StartCoroutine(Columns[column].SpawnFirstColumn());
    }
}
