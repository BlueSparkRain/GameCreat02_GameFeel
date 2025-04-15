public class GameInputModeManager : MonoSingleton<GameInputModeManager>
{
    //切换输入模式后禁用鼠标
    public E_Input inputType = E_Input.键鼠;


    /// <summary>
    /// 玩家主动切换输入模式
    /// </summary>
    /// <param name="type"></param>
    public void SwitchInputType(E_Input type)
    {
        switch (type)
        {
            case E_Input.键鼠:
                //显示鼠标
                break;
            case E_Input.手柄:
                //选择手柄，禁用鼠标，分配默认键
                break;
            default:
                break;
        }
    }
}

public enum E_Input
{
    键鼠, 手柄
}
