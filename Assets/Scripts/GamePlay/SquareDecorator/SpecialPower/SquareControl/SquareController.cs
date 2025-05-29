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
    S_ScaleCommand _scaleCommand;

    ISpecialPower squareSpecialPower;

    [ContextMenu("打印寻路任务")]
    private void PrintMask()
    {
        string sstr = "打印任务";
        for (int i = 0; i < (squareSpecialPower as SquareMoverToPlayerDecorator).moveTasks.Count; i++)
        {
            sstr += (" " + i + ":" + (squareSpecialPower as SquareMoverToPlayerDecorator).moveTasks[i]);
        }
        Debug.Log(sstr);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }


    /// <summary>
    /// 重置方块装饰器
    /// </summary>
    public void ReSetDecorator() 
    {
        squareSpecialPower = new BasePower();
    }

    public IEnumerator GetMoveToPlayerPower() 
    {
        yield return square.SquareToEnemyAnim();
        yield return null;
 
       squareSpecialPower = new SquareMoverToPlayerDecorator(squareSpecialPower,square.controller);
       
       gameObject.layer = 3;
       squareSpecialPower.PowerInit();
    }

    /// <summary>
    /// 已经获得某种功能
    /// </summary>
    bool isSuper;
    public void GetSuperMarkPower( E_SuperMarkType superType,bool isColDir,bool isRowDir) 
    {
        if (isSuper)
            return;
        isSuper=true;
        GameObject SuperMarkObj=null;
        SubCol col = square.slot.selfColumn;

        switch (superType)
        {
            case E_SuperMarkType.整行or整列:
        SuperMarkObj = Resources.Load<GameObject>("Prefab/SuperMark/Remove4Mark");
                squareSpecialPower = new SquareMarkRemoveDecorator(squareSpecialPower,SuperMarkObj,square, square.spRender.sprite,isColDir, isRowDir);
                break;
            case E_SuperMarkType.整行And整列:
        SuperMarkObj = Resources.Load<GameObject>("Prefab/SuperMark/Remove5Mark");
                squareSpecialPower = new SquareMarkRemoveDecorator(squareSpecialPower,SuperMarkObj,square, square.spRender.sprite,true,true);
                break;
            default:
                break;
        }

        squareSpecialPower.PowerInit();
    }

    public void RemoveDecoratorTrigger()
    {
        squareSpecialPower?.TriggerPower();
    }

    private void Update()
    {
        if(squareSpecialPower!=null)
        squareSpecialPower.PowerOnUpdate();
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

    public void InitSquare(Vector3 bornPos,Vector3 gravity ,Vector3 looseSpeed) 
    {
      SetSquarPos(bornPos);
      rb.SetCustomAcceleration(gravity);
      SetLooseSpeed(looseSpeed);
    }

    public void SetSquarPos(Vector3 targetPos) 
    {
       square.transform.position = targetPos;
       square.transform.localScale = Vector3.one*0.45f;
    }

    void SetLooseSpeed(Vector3 looseSpeed) 
    {
        _looseCommand.SetLooseSpeed(looseSpeed);
    }
    GameMap gamemap;

    private void Awake()
    {
        squareSpecialPower = new BasePower();
        gamemap = FindAnyObjectByType<GameMap>();
        rb = GetComponent<SimpleRigibody>();
        square ??= GetComponent<Square>();
        _moveTSlotCommand = new S_MoveTSlotCommand(square, rb, gamemap);
        _looseCommand = new S_LooseCommand(square, rb);

        if (GetComponent<ColorSquare>())
            _colorCommand = new S_ColorCommand(GetComponent<ColorSquare>());

        _removeCommand = new S_RemoveCommand(square);

        _pathMoveCommand = new S_PathMoveCommand(square, gamemap);

        _particalExplodeCommand = new S_ParticalExplodeCommand(square);
        _scaleCommand = new S_ScaleCommand(square);

    }



    /// <summary>
    /// 根据类型生成目标粒子
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
    /// 方块被销毁
    /// </summary>
    public void SqaureRemove() 
    {
        _removeCommand.Excute();
    }

    public IEnumerator SquareDoScale(Vector3 startValue, Vector3 endValue, float tranTine = 0.07f) 
    {
        yield return null;
        _scaleCommand.GetScaleTask(startValue,endValue,tranTine);
        _scaleCommand.Excute();
    }

    /// <summary>
    /// 方块移向父槽
    /// </summary>
    public void SquareMoveToSlot(Vector3 slotPos) 
    {
        if (transform.parent &&  transform.parent.GetComponent<WalkableSlot>())
        {

            StartCoroutine(MoveCheck());
            _moveTSlotCommand.GetTargetPos(slotPos);
            _moveTSlotCommand.Excute();
        }
    }
    IEnumerator MoveCheck()
    {
        isSwaping = true;
        yield return new WaitForSeconds(0.12f);
        isSwaping = false;
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
