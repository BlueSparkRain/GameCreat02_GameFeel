using System.Collections;
using UnityEngine;

public class SquareMarkRemoveDecorator :SquareDecorator
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

    protected Sprite markSprite;
    public SquareMarkRemoveDecorator(ISpecialPower power, GameObject MarkObj, Square selfSquare, Sprite MarkSprite, bool colDir, bool rowDir) : base(power)
    {
        this.MarkObj = MarkObj;
        this.selfSquare = selfSquare;
        markSprite = MarkSprite;
        isColDir = colDir;
        isRowDir = rowDir;
    }

    public override void PowerInit()
    {
        base.PowerInit();
        GameObject superMarkObj = GameObject.Instantiate(MarkObj, selfSquare.transform.position, Quaternion.identity, selfSquare.transform);


        superMarkObj.GetComponentInChildren<SuperMark>().SetSprite(markSprite);

        superMarkObj.transform.localPosition = Vector3.zero;
        superMarkObj.transform.localScale = Vector3.one * 1.1f;

        if (isColDir)
            superMarkObj.GetComponentInChildren<SuperMark>().ShowColArrow();
        if (isRowDir)
            superMarkObj.GetComponentInChildren<SuperMark>().ShowRowArrow();
    }
    protected IEnumerator RemoveWholeCol()
    {
        yield return new WaitForSeconds(0.5f);
        selfCol.RemoveWholeSubCol();
    }

    protected IEnumerator RemoveWholeRow()
    {
        yield return new WaitForSeconds(0.5f);
        selfRow.RemoveWholeRow();
    }

    public override void PowerOnUpdate()
    {
        base.PowerOnUpdate();
    }

    public override void TriggerPower()
    {
        base.TriggerPower();
        SuperPower();
    }

    protected virtual void SuperPower()
    {
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
            if (selfRow)
            {
                MonoManager.Instance.StartCoroutine(RemoveWholeRow());
            }
        }
    }

}

public enum E_SuperMarkType
{
    整行or整列,
    整行And整列,
}