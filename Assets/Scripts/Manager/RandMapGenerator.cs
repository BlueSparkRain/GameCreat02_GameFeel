using System.Collections.Generic;
using UnityEngine;

public static class RandMapGenerator 
{
   public static  int[,] GetRandomArray(int rows,int cols) 
    {
        ArrayGenerator generator = new ArrayGenerator();
        int[,] validArray = generator.GenerateValidArray(rows,cols);
        return validArray;
    }
}
