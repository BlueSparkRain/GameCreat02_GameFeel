using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMarkPower : IHaveSpecialPower
{
    /// <summary>
    /// 显示特征的方块能力
    /// </summary>
    protected GameObject MarkObj;
    protected Square selfSquare;

    protected SubCol selfCol;
    protected GameRow selfRow;
    protected bool isColDir;
    protected bool isRowDir;

    public SuperMarkPower(GameObject MarkObj,Square selfSquare) 
    {
        this.MarkObj = MarkObj;
        this.selfSquare = selfSquare;
    }
    public virtual void PowerInit()
    {
        GameObject superMarkObj=GameObject.Instantiate(MarkObj,selfSquare.transform.position,Quaternion.identity,selfSquare.transform);
        superMarkObj.transform.localPosition = Vector3.zero;
        superMarkObj.transform.localScale = Vector3.one*1.2f;

        if (isColDir)
            superMarkObj.GetComponentInChildren<SuperMark>().ShowColArrow();
        if (isRowDir)
            superMarkObj.GetComponentInChildren<SuperMark>().ShowRowArrow();

    }

    protected IEnumerator RemoveWholeCol()
    {
        yield return new WaitForSeconds(0.5f);
        //PostProcessManager.Instance.LenDistortionFlash();
        
        selfCol.RemoveWholeSubCol();
    }

    protected IEnumerator RemoveWholeRow()
    {
        yield return new WaitForSeconds(0.5f);

        //PostProcessManager.Instance.LenDistortionFlash();
        selfRow.RemoveWholeRow();
    }

    public void PowerOnUpdate()
    {

    }

    public void TriggerPower()
    {
        SuperPower();
    }

    protected virtual void SuperPower() 
    {
    
    
    }


}

public enum E_SuperMarkType 
{
   整行or整列,
   整行And整列,
}

