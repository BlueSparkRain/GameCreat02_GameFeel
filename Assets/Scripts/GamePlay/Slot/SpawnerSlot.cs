
using System.Collections;
using UnityEngine;

public class SpawnerSlot : Slot
{
    Vector3 gravityDir;
    Vector3 looseSpeed;

    //�������·��������
    SquareController newSquareController;

    SquarePoolManager squarePool;

    public void Init(Vector3 gravityDir, Vector3 looseSpeed)
    {
        squarePool = FindAnyObjectByType<SquarePoolManager>();
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
        GameObject newSquare = squarePool.GetRandomColorSquare();
        ColFallOneSquare(newSquare);
        //StartCoroutine(ColFallOneSquare(newSquare));
    }

  
    /// <summary>
    /// ����������һ��Ŀ����ɫɫ�飬��ʼ��������
    /// </summary>
    /// <param name="so"></param>
    /// <returns></returns>
    public GameObject SubColAddTargetColorSquare(ColorSquareSO so) 
    {
        GameObject newSquare = squarePool.GetTargetColorSquare(so);
        //StartCoroutine(ColFallOneSquare(newSquare));
        ColFallOneSquare(newSquare);
        return newSquare;
    }

    public GameObject SubColAddSpecialSquare(E_SpecialSquareType squareType)
    {
        GameObject newSquare = squarePool.GetTargetSpecialSquare(squareType);
        switch (squareType)
        {
            case E_SpecialSquareType.���ڷ���:

                break;
            case E_SpecialSquareType.�����ŷ���:

                break;
        }
        return newSquare;

    }

    ///// <summary>
    ///// ��һ�������ʼ������׹
    ///// </summary>
    ///// <param name="square"></param>
    ///// <returns></returns>
    //public IEnumerator ColFallOneSquare(GameObject square)
    //{
    //    yield return null;
    //    newSquareController = square.GetComponent<SquareController>();
    //    newSquareController?.InitSquare(transform.position, gravityDir, looseSpeed);
    //    newSquareController?.SquareLoose();//���·���
    //}

    public void BornSquareToTargetSlot(WalkableSlot slot)
    {
        GameObject newSquare = squarePool.GetRandomColorSquare();

        newSquareController = newSquare.GetComponent<SquareController>();
        newSquareController?.InitSquare(slot.transform.position, gravityDir, looseSpeed);
        slot.SetSquare(newSquareController.square);
        newSquareController?.SquareDoScale(Vector3.zero,Vector3.one*0.45f);//����

    }

    /// <summary>
    /// ��һ�������ʼ������׹
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    public void ColFallOneSquare(GameObject square)
    {
        newSquareController = square.GetComponent<SquareController>();
        newSquareController?.InitSquare(transform.position, gravityDir, looseSpeed);
        newSquareController?.SquareLoose();//���·���
    }

}


