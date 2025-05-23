using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Super5RemovePower : SuperMarkPower
{
    public Super5RemovePower(GameObject MarkObj, Square selfSquare, bool colDir, bool rowDir) : base(MarkObj, selfSquare)
    {
        isColDir = colDir;
        isRowDir = rowDir;
    }

    protected override void SuperPower()
    {
        Debug.Log("消除整行+整列");
        base.SuperPower();

        selfCol = selfSquare.slot.selfColumn;
        if (selfCol)
        {
            MonoManager.Instance.StartCoroutine(RemoveWholeCol());
        }
        selfRow = GameObject.FindAnyObjectByType<GameMap>().Rows[selfSquare.slot.NodeIndex.y];
        if (selfRow)
        {
            MonoManager.Instance.StartCoroutine(RemoveWholeRow());
        }
    }


}
