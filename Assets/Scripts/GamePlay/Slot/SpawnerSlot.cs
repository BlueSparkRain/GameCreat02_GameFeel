
using System.Collections;
using UnityEngine;

public class SpawnerSlot : Slot
{
    Vector3 gravityDir;
    Vector3 looseSpeed;

    //本列最新方块控制器
    SquareController newSquareController;

    SquarePoolManager squarePool;
    private void Awake()
    {
        Debug.Log("傻傻的拉到");
        squarePool = SquarePoolManager.Instance;
    }

    public void Init(Vector3 gravityDir, Vector3 looseSpeed)
    {
        squarePool =SquarePoolManager.Instance;
        this.gravityDir = gravityDir;
        this.looseSpeed = looseSpeed;
    }

    public void SetGravity(Vector3 gravityDir) 
    {
        this.gravityDir = gravityDir;
    }

    public void SetLooseSpeed(Vector3 looseSpeed)
    {
        this.looseSpeed = looseSpeed;
    }

    /// <summary>
    /// 本列新增加一个随机颜色色块，初始化并落下
    /// </summary>
    public void SubColAddOneRandomSquare()
    {
        GameObject newSquare = squarePool.GetRandomColorSquare();
        ColFallOneSquare(newSquare);
    }

    /// <summary>
    /// 本列新增加一个目标颜色色块，初始化并落下
    /// </summary>
    /// <param name="so"></param>
    /// <returns></returns>
    public GameObject SubColAddTargetColorSquare(ColorSquareSO so) 
    {
        GameObject newSquare = squarePool.GetTargetColorSquare(so);
        ColFallOneSquare(newSquare);
        return newSquare;
    }

    /// <summary>
    /// 本列新增加一个目标特殊块，初始化并落下
    /// </summary>
    /// <param name="squareType"></param>
    /// <returns></returns>
    public GameObject SubColAddSpecialSquare(E_SpecialSquareType squareType,int taskIndex=0)
    {
        GameObject newSquare = squarePool.GetSpecialSquare(squareType,taskIndex);
        ColFallOneSquare(newSquare);
        return newSquare;
    }

    public IEnumerator BornSquareToTargetSlot(WalkableSlot slot)
    {
        GameObject newSquare = squarePool.GetRandomColorSquare();

        newSquareController = newSquare.GetComponent<SquareController>();
        newSquareController?.InitSquare(slot.transform.position, gravityDir, looseSpeed);
        slot.SetSquare(newSquareController.square);
        yield return newSquareController?.SquareDoScale(Vector3.zero,Vector3.one*1.56f);//方块
    }

    /// <summary>
    /// 让一个方块初始化并下坠
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    public void ColFallOneSquare(GameObject square)
    {
        newSquareController = square.GetComponent<SquareController>();
        newSquareController?.InitSquare(transform.position, gravityDir, looseSpeed);
        newSquareController?.SquareLoose();//落下方块
    }

}


