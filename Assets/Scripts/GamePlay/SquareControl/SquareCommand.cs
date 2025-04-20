using UnityEngine;

public class SquareCommand : ICommand
{
    protected readonly Square square;
    protected Transform self;
    protected Transform father;
    protected Slot slot;
    protected MonoManager mono;
    protected SquareGroup  squareGroup;    

    public SquareCommand(Square square,SquareGroup squareGroup)
    {
        this.square = square;
        this.squareGroup = squareGroup;
        self = square.transform;
        mono = MonoManager.Instance;
    }

    public virtual void Excute()
    {

    }
}
