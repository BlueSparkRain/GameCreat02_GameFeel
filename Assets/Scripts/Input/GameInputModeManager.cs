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
                //��ʾ���
                break;
            case E_Input.�ֱ�:
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
