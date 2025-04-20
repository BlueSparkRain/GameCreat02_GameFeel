using UnityEngine;

public class SquareController : MonoBehaviour
{
    public Square square;
    SimpleRigibody rb;
    SquareGroup  squareGroup;

    S_MoveTSlotCommand  _moveTSlotCommand;
    S_LooseCommand _looseCommand;
    S_ColorCommand _colorCommand;
    S_RemoveCommand _removeCommand;

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

    public void SetLooseSpeed(Vector3 looseSpeed) 
    {
       square.SetLooseSpeed(looseSpeed);
    }

    private void Start()
    {
        squareGroup =  FindAnyObjectByType<SquareGroup>();
        rb = GetComponent<SimpleRigibody>();
        square ??=GetComponent<Square>();
        _moveTSlotCommand =new S_MoveTSlotCommand(square,squareGroup,rb);
        _looseCommand = new S_LooseCommand(square, squareGroup);
        _colorCommand = new S_ColorCommand(GetComponent<ColorSquare>(),squareGroup);
        _removeCommand=new S_RemoveCommand(square,squareGroup);
    }

    /// <summary>
    /// 方块被销毁
    /// </summary>
    public void SqaureRemove() 
    {
        _removeCommand.Excute();

    }
    

    /// <summary>
    /// 方块移向父槽
    /// </summary>
    public void SquareMoveToSlot() 
    {
        if (transform.parent &&  transform.parent.GetComponent<Slot>())
        {
            _moveTSlotCommand.Excute();
        }
    }

    /// <summary>
    /// 方块松落
    /// </summary>
    public void SquareLoose() 
    {
       _looseCommand.Excute();
    }

    /// <summary>
    /// 色块染色
    /// /// </summary>
    public void SquareColor() 
    {
      _colorCommand.Excute();
    }
}
