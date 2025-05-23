using UnityEngine;
public class S_MoveTSlotCommand : SquareCommand
{
    GameMap gameMap;
    public S_MoveTSlotCommand(Square square, SimpleRigibody rb, GameMap gameMap) : base(square)
    {
        this.rb = rb;
        this.gameMap = gameMap;

    }
    Vector3 truePos;
    Vector3 targetPos;
    SimpleRigibody rb;

    public void GetTargetPos(Vector3 slotPos)
    {
        targetPos = slotPos;
    }
    public override void Excute()
    {
        base.Excute();
        MoveToTargetPos();
    }

    void MoveToTargetPos()
    {
        controlSquare.HasFather = true;
        rb.GetSlot();
        controlFather = controlSquare.transform.parent;
        //truePos = new Vector3(targetPos.x, targetPos.y, -0.1f);
        truePos = targetPos;

        if (controlFather != null && controlFather.GetComponent<WalkableSlot>() != null)
        {
            controlSquare.SetSlot(controlFather.GetComponent<WalkableSlot>());
            controlSlot = controlSquare.slot;
            controlSlot.selfColumn.UpdateSubColumnSquares(controlSquare, controlSlot.transform.GetSiblingIndex());
            gameMap.UpdateRowSquares(controlSquare, controlSlot.transform.parent.parent.GetSiblingIndex(), controlSlot.mapIndex);
        }
        mono.StartCoroutine(TweenHelper.MakeLerp(controlSelf.position, truePos, 0.1f, val => controlSelf.position = val));

    }


}
