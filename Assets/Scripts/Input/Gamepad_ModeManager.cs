using UnityEngine;
using UnityEngine.EventSystems;

public class Gamepad_ModeManager : MonoSingleton<Gamepad_ModeManager>
{
    public  GameObject currentSelectObj;//当前选中物体

    /// <summary>
    /// 将 手柄选择当前的对象 指定给EventSystem【每次打开面板时调用】
    /// </summary>  
    public void SetEventSystemCurrentObj(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(obj);
        currentSelectObj = obj;//此处currentSelectObj表示本面板的默认选择对象
    }

    /// <summary>
    /// 在打开面板后，进行移动输入时调用
    /// </summary> 
    public void SetSelfCurrentObj() 
    {
        //由于按钮的自动导航，在面板上浏览时，currentSelectObj跟进赋值
        currentSelectObj=EventSystem.current.currentSelectedGameObject;
    }


    private void Update()
    {
        if( currentSelectObj!=null && currentSelectObj!= EventSystem.current.currentSelectedGameObject)
            SetSelfCurrentObj();

    }


}


