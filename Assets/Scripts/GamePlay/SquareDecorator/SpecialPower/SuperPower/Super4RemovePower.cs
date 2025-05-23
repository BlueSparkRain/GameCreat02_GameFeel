using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Super4RemovePower : SuperMarkPower
{
    //bool isColDir;
    public Super4RemovePower(GameObject MarkObj, Square selfSquare,bool colDir,bool rowDir) : base(MarkObj, selfSquare)
    {
        isColDir = colDir;
        isRowDir = rowDir;
            
    }

    protected override void SuperPower()
    {
        base.SuperPower();
        if (isColDir)
        {
            selfCol = selfSquare.slot.selfColumn;
            if (selfCol)
            {
                MonoManager.Instance.StartCoroutine(RemoveWholeCol());
            }
        }

        if (isRowDir)
        {
            selfRow = GameObject.FindAnyObjectByType<GameMap>().Rows[selfSquare.slot.NodeIndex.y];
            //selfRow = selfSquare.slot.selfColumn.GetComponentInParent<GameRow>();
            if (selfRow)
            {
                MonoManager.Instance.StartCoroutine(RemoveWholeRow());
            }
        }
    }

    
}
