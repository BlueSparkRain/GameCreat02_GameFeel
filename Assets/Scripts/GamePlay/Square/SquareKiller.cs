using System.Collections;
using UnityEngine;

public class SquareKiller : MonoBehaviour
{
    SquareObjPool pool;
    SquareColumn col;

    private void Start()
    {
        col=GetComponentInParent<SquareColumn>();
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

            //if ( col.SquareNum < 8)
            //{
            //    //col.ColumnAddOneSquare();
            //    //Debug.Log("Â©ÍøÖ®Óã");
            //    StartCoroutine(Wait());
            //}
            
            pool.ReturnPool(other.gameObject.GetComponent<ColorSquare>());
        }
    }
    //IEnumerator Wait() 
    //{
    //    yield return new WaitForSeconds(2f);
    //    col.ColumnAddOneRandomSquare();
    //    Debug.Log("Â©ÍøÖ®Óã");
    //}
}
