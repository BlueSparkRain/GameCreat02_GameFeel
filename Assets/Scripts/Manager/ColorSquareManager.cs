using System.Collections.Generic;
using UnityEngine;

public class ColorSquareManager : MonoSingleton<ColorSquareManager>
{
    void Start()
    {
        ArrayGenerator generator = new ArrayGenerator();
        int[,] validArray = generator.GenerateValidArray();
        generator.PrintArray(validArray);
    }

    int[,] GetRandomArray() 
    {
        ArrayGenerator generator = new ArrayGenerator();
        int[,] validArray = generator.GenerateValidArray();
        return validArray;
    }

    public void BornAllSquares() 
    {
        int[,] validArray = GetRandomArray();

        for (int i = 0; i < 8; i++) 
        {
            List<int> intList = new List<int>();

            for (int j = 0; j < 8; j++)
            {
                intList.Add(validArray[i, j]);
            }

            List<ColorSquareSO> soList= FindAnyObjectByType<SquareObjPool>().GetColorSOList(intList);
            FindAnyObjectByType<SquareGroup>().FirstColSquares(i,soList);
        }

    }

}
