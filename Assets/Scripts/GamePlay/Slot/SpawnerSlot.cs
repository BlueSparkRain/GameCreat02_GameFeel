
using System.Collections;
using UnityEngine;

public class SpawnerSlot : Slot
{
    Vector3 gravityDir;
    Vector3 looseSpeed;

    //�������·��������
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
    /// ����������һ�������ɫɫ�飬��ʼ��������
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
    /// ��һ�������ʼ������׹
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    public IEnumerator ColFallOneSquare(GameObject square)
    {
        yield return null;
        newSquareController = square.GetComponent<SquareController>();
        newSquareController?.InitSquare(transform.position, gravityDir, looseSpeed);
        newSquareController?.SquareLoose();//���·���
    }

}
