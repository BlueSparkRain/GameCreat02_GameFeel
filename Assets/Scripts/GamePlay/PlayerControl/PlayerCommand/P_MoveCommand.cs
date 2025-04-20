public class P_MoveCommand : Player_Command
{
    public P_MoveCommand(Player player) : base(player)
    {

    }

    public override void Excute()
    {
        base.Excute();
        MoveOnce();
    }

    public void MoveOnce()
    {
        targetSquare = player.CheckTarget(player.targetDir);
        if (targetSquare && targetSquare.transform.parent?.GetComponent<Slot>())
           mono.StartCoroutine(player.Swap(targetSquare?.GetComponent<Square>()));
    }
}
