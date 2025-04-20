using UnityEngine;
public class S_MoveTSlotCommand : SquareCommand
{
    public S_MoveTSlotCommand(Square square,  SquareGroup squareGroup,SimpleRigibody rb) : base(square, squareGroup)
    {
        this.rb = rb;
    }
    Vector3 targetPos;
    SimpleRigibody rb;
    public override void Excute()
    {
        base.Excute();
        MoveToSlot();
    }

    void MoveToSlot()
    {
        father = self.parent;
        if (father && father.GetComponent<Slot>())
        {
            self.SetParent(null);
            return;
        }

        rb.SetZeroSpeed();
        square.HasFather = true;

        targetPos = new Vector3(father.position.x, father.position.y, -0.1f);

        slot = father.GetComponent<Slot>();
        slot.selfColumn.UpdateColumnSquares(square, father.GetSiblingIndex());
        squareGroup.UpdateRowSquares(square, father.parent.GetSiblingIndex(), father.GetSiblingIndex());
    }
}
