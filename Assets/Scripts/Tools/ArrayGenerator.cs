using System;
using UnityEngine;

public class ArrayGenerator
{
    private readonly int rows = 8;
    private readonly int cols = 8;
    private readonly int minNumber = 1;
    private readonly int maxNumber = 6;
    //private readonly int maxNumber = 3;
    private readonly System.Random rand = new System.Random();

    public int[,] GenerateValidArray()
    {
        int[,] grid = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int candidate;
                do
                {
                    candidate = rand.Next(minNumber, maxNumber + 1);
                } while (!IsValidCandidate(grid, i, j, candidate));

                grid[i, j] = candidate;
            }
        }
        return grid;
    }

    private bool IsValidCandidate(int[,] grid, int row, int col, int candidate)
    {
        // �����飨�������Ԫ�أ�
        if (col >= 2 &&
            grid[row, col - 1] == candidate &&
            grid[row, col - 2] == candidate)
            return false;

        // �����飨�Ϸ�����Ԫ�أ�
        if (row >= 2 &&
            grid[row - 1, col] == candidate &&
            grid[row - 2, col] == candidate)
            return false;

        return true;
    }

    // ��ӡ����ĸ�������
    public void PrintArray(int[,] grid)
    {
        //for (int i = 0; i < rows; i++)
        //{
        //    for (int j = 0; j < cols; j++)
        //    {
        //        Debug.Log(grid[i, j]);
        //    }
        //    Console.WriteLine();
        //}
    }

    // ʾ���÷�
    public static void Main()
    {
        ArrayGenerator generator = new ArrayGenerator();
        int[,] validArray = generator.GenerateValidArray();
        generator.PrintArray(validArray);
    }
}
