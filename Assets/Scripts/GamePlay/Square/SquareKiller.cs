using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareKiller : MonoBehaviour
{
    SquareObjPool pool;

    private void Start()
    {
        pool = FindAnyObjectByType<SquareObjPool>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<ColorSquare>())
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                other.gameObject.transform.position += new Vector3(0, 100, 0);
                return;
            }
            pool.ReturnPool(other.gameObject.GetComponent<ColorSquare>()); 
        }
    }
}
