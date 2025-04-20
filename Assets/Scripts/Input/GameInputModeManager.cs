public class GameInputModeManager : MonoSingleton<GameInputModeManager>
{
    //�л�����ģʽ��������
    public E_Input inputType = E_Input.����;

    /// <summary>
    /// ��������л�����ģʽ
    /// </summary>
    /// <param name="type"></param>
    public void SwitchInputType(E_Input type)
    {
        switch (type)
        {
            case E_Input.����:
                inputType = E_Input.����;
                Keyboard_Mouse_ModeManager.Instance.FreezeCursor();
                //��ʾ���
                break;
            case E_Input.�ֱ�:
                inputType = E_Input.�ֱ�;
                Keyboard_Mouse_ModeManager.Instance.ExitMouseInputMode();
                 //Gamepad_ModeManager.Instance.
                 //ѡ���ֱ���������꣬����Ĭ�ϼ�
                break;
            default:
                break;
        }
    }
}

public enum E_Input
{
    ����, �ֱ�
}
