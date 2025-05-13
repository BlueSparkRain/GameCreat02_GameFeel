
using System.Collections;
using UnityEngine;

public class SpawnerSlot : Slot
{
    Vector3 gravityDir;
    Vector3 looseSpeed;

    //本列最新方块控制器
    SquareController newSquareController;


    SquareObjPool squarePool;

    public void Init(Vector3 gravityDir, Vector3 looseSpeed)
    {
        squarePool = FindAnyObjectByType<SquareObjPool>();
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
        GameObject newSquare = squarePool.GetRandomSquare();
        StartCoroutine(ColFallOneSquare(newSquare));
    }

    public GameObject SubColAddTargetSquare(ColorSquareSO so) 
    {
        GameObject newSquare = squarePool.GetTargetSquare(so);
        StartCoroutine(ColFallOneSquare(newSquare));
        return newSquare;
    }

    /// <summary>
    /// 让一个方块初始化并下坠
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    public IEnumerator ColFallOneSquare(GameObject square)
    {
        yield return null;
        newSquareController = square.GetComponent<SquareController>();
        newSquareController?.InitSquare(transform.position, gravityDir, looseSpeed);
        newSquareController?.SquareLoose();//落下方块
    }

}
