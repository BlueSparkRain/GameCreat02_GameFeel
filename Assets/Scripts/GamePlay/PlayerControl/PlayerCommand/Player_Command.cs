using UnityEngine;
public class Player_Command:ICommand
{
    protected readonly Player player;
    protected Transform self;
    protected GameObject targetSquare;
    protected MonoManager mono;

    public Player_Command(Player player)
    {
        this.player = player;
        self=player.transform;
        mono ??= MonoManager.Instance;
    }

    public virtual void Excute()
    {
        if (self.parent == null)
            return;
    }

}
