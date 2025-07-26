using UnityEngine;
public class S_LooseCommand : SquareCommand
{
    public S_LooseCommand(Square square, SimpleRigibody rb) : base(square){
        this.rb = rb;
    }

    Vector3 looseSpeed;
    SimpleRigibody rb;
    public override void Excute()
    {
        base.Excute();
        LooseSelf();
    }

    public void SetLooseSpeed(Vector3 looseSpeed) 
    {
        this.looseSpeed = looseSpeed;
    }

    public virtual void LooseSelf()
    {
        controlSelf.SetParent(null);
        controlSlot = null;
        controlSquare.HasFather = false;
        rb.SetLooseVelocity(looseSpeed);
    }
}
