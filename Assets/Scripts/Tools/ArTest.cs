using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ArrayGenerator generator = new ArrayGenerator();
        int[,] validArray = generator.GenerateValidArray();
        generator.PrintArray(validArray);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
