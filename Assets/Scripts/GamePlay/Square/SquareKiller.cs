using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareKiller : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Square>())
        {
            //Debug.Log("ТЉЭјжЎгу");
            other.gameObject.GetComponent<Square>().BeRemoved();
            FindAnyObjectByType<SquareObjPool>().ReturnPool(other.gameObject.GetComponent<ColorSquare>()); 
        }
    }
}
