using UnityEngine;

public class SquareCommand : ICommand
{
    protected readonly Square controlSquare;
    protected Transform controlSelf;
    protected Transform controlFather;
    protected WalkableSlot controlSlot;
    protected MonoManager mono;

    public SquareCommand(Square square)
    {
        controlSquare = square;
        controlSelf = square.transform;
        mono = MonoManager.Instance;
    }

    public virtual void Excute()
    {

    }
}
