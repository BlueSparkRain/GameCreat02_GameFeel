public class P_ColorationCommand : Player_Command
{
    public P_ColorationCommand(Player player) : base(player){
    }

    public override void Excute()
    {
        base.Excute();
        ColorationOnce();
    }

    public void ColorationOnce()
    {
        targetSquare = player.CheckTarget(player.targetDir);
        if (targetSquare && targetSquare?.GetComponent<ColorSquare>())
            player.Coloration(targetSquare?.GetComponent<ColorSquare>());
    }

}
