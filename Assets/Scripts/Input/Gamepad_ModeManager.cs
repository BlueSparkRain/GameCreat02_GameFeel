using UnityEngine;
using UnityEngine.EventSystems;

public class Gamepad_ModeManager : MonoSingleton<Gamepad_ModeManager>
{
    public  GameObject currentSelectObj;//��ǰѡ������

    /// <summary>
    /// �� �ֱ�ѡ��ǰ�Ķ��� ָ����EventSystem��ÿ�δ����ʱ���á�
    /// </summary>  
    public void SetEventSystemCurrentObj(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(obj);
        currentSelectObj = obj;//�˴�currentSelectObj��ʾ������Ĭ��ѡ�����
    }

    /// <summary>
    /// �ڴ����󣬽����ƶ�����ʱ����
    /// </summary> 
    public void SetSelfCurrentObj() 
    {
        //���ڰ�ť���Զ�����������������ʱ��currentSelectObj������ֵ
        currentSelectObj=EventSystem.current.currentSelectedGameObject;
    }


    private void Update()
    {
        if( currentSelectObj!=null && currentSelectObj!= EventSystem.current.currentSelectedGameObject)
            SetSelfCurrentObj();

    }


}


