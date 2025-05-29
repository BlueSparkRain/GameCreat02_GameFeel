
using System.Collections;
using UnityEngine;

public class SpawnerSlot : Slot
{
    Vector3 gravityDir;
    Vector3 looseSpeed;

    //�������·��������
    SquareController newSquareController;

    SquarePoolManager squarePool;
    private void Awake()
    {
        Debug.Log("ɵɵ������");
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
    /// ����������һ�������ɫɫ�飬��ʼ��������
    /// </summary>
    public void SubColAddOneRandomSquare()
    {
        GameObject newSquare = squarePool.GetRandomColorSquare();
        ColFallOneSquare(newSquare);
    }

    /// <summary>
    /// ����������һ��Ŀ����ɫɫ�飬��ʼ��������
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
    /// ����������һ��Ŀ������飬��ʼ��������
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
        yield return newSquareController?.SquareDoScale(Vector3.zero,Vector3.one*1.56f);//����
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


