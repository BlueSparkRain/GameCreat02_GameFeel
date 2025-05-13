using System.Collections.Generic;
using UnityEngine;

public class ColorSquareManager : MonoSingleton<ColorSquareManager>
{

    void Start()
    {
        //ArrayGenerator generator = new ArrayGenerator();
        //int[,] validArray = generator.GenerateValidArray();
        //generator.PrintArray(validArray);
    }

    int[,] GetRandomArray(int rows,int cols) 
    {
        ArrayGenerator generator = new ArrayGenerator();
        int[,] validArray = generator.GenerateValidArray(rows,cols);
        return validArray;
    }

    public void BornAllSquares(int W) 
    {
        int[,] validArray = GetRandomArray(W,W);

        for (int i = 0; i < W; i++)
        {
            List<int> intList = new List<int>();

            for (int j = 0; j < W; j++)
            {
                intList.Add(validArray[i, j]);
            }

            List<ColorSquareSO> soList = FindAnyObjectByType<SquareObjPool>().GetColorSOList(intList);

            ///≤‚ ‘
            //string strs="¡–£∫"+i.ToString()+":";
            //for (int j = 0; j < intList.Count; j++)
            //{
            //    strs+=intList[j].ToString()+soList[j].E_Color+",";
            //}
            //Debug.Log(strs);
            ///


            FindAnyObjectByType<GameMap>().FirstColSquares(i,soList);
        }

    }

}
