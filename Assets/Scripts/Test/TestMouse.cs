using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouse : MonoBehaviour
{
    private void OnMouseEnter()
    {
        transform.localScale = Vector3.one*1.5f;
    }

    private void OnMouseExit()
    {
        transform.localScale = Vector3.one;
        
    }
}
