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
        targetSquare = RayChecker.CheckTargetLayerObj(player.whatIsSquare,player.targetSquareChecckDistance,  player.transform.position, player.targetDir);
        targetSquare ??= RayChecker.CheckTargetLayerObj(player.whatIsEnemy, player.targetSquareChecckDistance, player.transform.position, player.targetDir);
        if (targetSquare && targetSquare.transform.parent?.GetComponent<WalkableSlot>())
           //mono.StartCoroutine(player.Swap(targetSquare?.GetComponent<Square>()));
        mono.StartCoroutine(player.Swap(targetSquare?.GetComponent<SquareController>()));
    }
}
