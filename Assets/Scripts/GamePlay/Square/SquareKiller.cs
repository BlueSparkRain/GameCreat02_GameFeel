using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareKiller : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<ColorSquare>())
        {
            //Debug.Log("ТЉЭјжЎгу");
            //Destroy(other.gameObject);
            //other.gameObject.GetComponent<Square>().BeRemoved();
            FindAnyObjectByType<SquareObjPool>().ReturnPool(other.gameObject.GetComponent<ColorSquare>()); 
        }
    }
}
