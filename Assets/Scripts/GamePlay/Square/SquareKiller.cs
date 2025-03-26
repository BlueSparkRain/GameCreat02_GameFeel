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
            if (other.gameObject.GetComponent<PlayerController>())
            {
                other.gameObject.transform.position += new Vector3(0, 100, 0);
                return;
            }
            FindAnyObjectByType<SquareObjPool>().ReturnPool(other.gameObject.GetComponent<ColorSquare>()); 
        }
    }
}
