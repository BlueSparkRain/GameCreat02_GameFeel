using System.Collections;
using UnityEngine;

public class SquareKiller : MonoBehaviour
{
    WholeObjPoolManager pool;

    private void Start()
    {
        pool ??= WholeObjPoolManager.Instance;
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

        
            pool.ObjReturnPool(E_ObjectPoolType.É«¿é³Ø, other.gameObject);
        }
    }
}
