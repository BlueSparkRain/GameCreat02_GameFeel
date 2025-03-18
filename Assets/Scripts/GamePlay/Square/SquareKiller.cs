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
            //Debug.Log("Â©ÍøÖ®Óã");
            other.gameObject.GetComponent<Square>().BeRemoved();
            FindAnyObjectByType<SquareObjPool>().ReturnPool(other.gameObject.GetComponent<ColorSquare>());

            //if (transform.parent.GetComponent<SquareColumn>())
            //{
                SquareColumn col = transform.parent.GetComponent<SquareColumn>();

                if (col.ColFull)
                {
                    if (col.SquareNum != 8 || col.FirstEmptySlotIndex != 0)
                    {
                        for (int i = 0; i < col.FirstEmptySlotIndex; i++)
                            StartCoroutine(col.ColumnAddOneSquare());
                    }

                    if (col.SquareNum != 8 && col.FirstEmptySlotIndex != 0)
                    //if (col.FirstEmptySlotIndex != 0)
                    {
                        for (int i = 0; i < col.FirstEmptySlotIndex; i++)
                            StartCoroutine(col.ColumnAddOneSquare());
            
                    }

                }
            }

        //}
    }
}
