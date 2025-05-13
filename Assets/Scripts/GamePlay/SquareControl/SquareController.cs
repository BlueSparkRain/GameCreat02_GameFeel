using System.Collections;
using UnityEngine;

public class SquareController : MonoBehaviour
{
    public Square square;
    SimpleRigibody rb;

    S_MoveTSlotCommand  _moveTSlotCommand;
    S_LooseCommand _looseCommand;
    S_ColorCommand _colorCommand;
    S_RemoveCommand _removeCommand;
    S_PathMoveCommand _pathMoveCommand;
    S_ParticalExplodeCommand _particalExplodeCommand;

    SquareDecorator squareDecorator;


    [ContextMenu("��ӡѰ·����")]
    private void PrintMask()
    {
        string sstr = "��ӡ����";
        for (int i = 0; i < (squareDecorator. iAmSpecial as ToPlayerMovePower).moveTasks.Count; i++)
        {
            sstr += (" " + i + ":" + (squareDecorator.iAmSpecial as ToPlayerMovePower).moveTasks[i]);
        }
        Debug.Log(sstr);

    }

    /// <summary>
    /// ���÷���װ����
    /// </summary>
    public void ReSetDecorator() 
    {
        //Debug.Log("��ʧȥ��Ȩ��");
        squareDecorator = null;
    }

    public void GetMovePower() 
    {
      squareDecorator=new SquareMoverDecorator(new ToPlayerMovePower(this));
      gameObject.layer = 3;
      squareDecorator.PowerInit();
      //Debug.Log("׷����");
    }

    private void Update()
    {
        if(squareDecorator!=null)
        squareDecorator.PowerOnUpdate();
    }

    public bool isSwaping;
    public void OnSwap() 
    {
     isSwaping = true;
    }
    public void ExitSwap() 
    {
     isSwaping=false;
    }

    /// <summary>
    /// �����Ѿ������ڳ����еķ���
    /// </summary>
    public void PrepareSquareInit() 
    {
        if (transform.parent && transform.parent.GetComponent<WalkableSlot>())
        {
           SquareMoveToSlot(transform.parent.position);
           square.slot.transform.parent.parent.GetChild(transform.parent.GetSiblingIndex()).GetComponent<GameMap>().UpdateRowSquares(square, square.slot.transform.parent.parent.GetSiblingIndex(),  square.slot.mapIndex);
        }

    }

    public void InitSquare(Vector3 bornPos,Vector3 gravity ,Vector3 looseSpeed) 
    {
      SetSquarPos(bornPos);
      rb.SetCustomAcceleration(gravity);
      SetLooseSpeed(looseSpeed);
    }

    public void SetSquarPos(Vector3 targetPos) 
    {
       square.transform.position = targetPos;
    }

    void SetLooseSpeed(Vector3 looseSpeed) 
    {
        _looseCommand.SetLooseSpeed(looseSpeed);
    }
    GameMap gamemap;

    private void Awake()
    {
        gamemap = FindAnyObjectByType<GameMap>();
        rb = GetComponent<SimpleRigibody>();
        square ??= GetComponent<Square>();
        _moveTSlotCommand = new S_MoveTSlotCommand(square, rb, gamemap);
        _looseCommand = new S_LooseCommand(square, rb);
        _colorCommand = new S_ColorCommand(GetComponent<ColorSquare>());
        _removeCommand = new S_RemoveCommand(square);
        _pathMoveCommand = new S_PathMoveCommand(square, gamemap);
        _particalExplodeCommand = new S_ParticalExplodeCommand(square);
    }

    private void Start()
    {
        //gamemap = FindAnyObjectByType<GameMap>();
        //rb = GetComponent<SimpleRigibody>();
        //square ??=GetComponent<Square>();
        //_moveTSlotCommand =new S_MoveTSlotCommand(square,rb, gamemap);
        //_looseCommand = new S_LooseCommand(square,rb);
        //_colorCommand = new S_ColorCommand(GetComponent<ColorSquare>());
        //_removeCommand=new S_RemoveCommand(square);
        //_pathMoveCommand = new S_PathMoveCommand(square, gamemap);
        //_particalExplodeCommand = new S_ParticalExplodeCommand(square);
    }


    /// <summary>
    /// ������������Ŀ������
    /// </summary>
    /// <param name="particalType"></param>
    public void SquareCreateTargetPartical(E_ParticalType particalType) 
    {
        _particalExplodeCommand.GetParticalType(particalType);
        _particalExplodeCommand.Excute();

    }

    public void SquareMoveToTargetDir(E_CustomDir moveDir) 
    {
        _pathMoveCommand.TryGetSwapTarget(moveDir);
        _pathMoveCommand.Excute();
    }


    /// <summary>
    /// ���鱻����
    /// </summary>
    public void SqaureRemove() 
    {
        _removeCommand.Excute();

    }
    

    /// <summary>
    /// �������򸸲�
    /// </summary>
    public void SquareMoveToSlot(Vector3 slotPos) 
    {
        if (transform.parent &&  transform.parent.GetComponent<WalkableSlot>())
        {

            StartCoroutine(Swap());
            _moveTSlotCommand.GetTargetPos(slotPos);
            _moveTSlotCommand.Excute();
        }
    }
    IEnumerator Swap()
    {
        isSwaping = true;
        yield return new WaitForSeconds(0.12f);
        isSwaping = false;


    }

    /// <summary>
    /// ��������
    /// </summary>
    public void SquareLoose() 
    {
       _looseCommand.Excute();
    }

    /// <summary>
    /// ɫ��Ⱦɫ
    /// /// </summary>
    public void SquareColor() 
    {
      _colorCommand.Excute();
    }
}
