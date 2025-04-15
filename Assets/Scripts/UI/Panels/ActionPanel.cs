using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour
{
    public Dropdown dropDown;

    void Start()
    {
        dropDown.onValueChanged.AddListener(OnDropDownValueChange);
    }


    /// <summary>
    /// 为下拉菜单配置切换逻辑
    /// </summary>
    /// <param name="val">更改值</param>
    void OnDropDownValueChange(int val) 
    {
        switch (val)
        {
            case 0:
                GameInputModeManager.Instance.SwitchInputType(E_Input.键鼠);
                break;
            case 1:
                GameInputModeManager.Instance.SwitchInputType(E_Input.手柄);
                break;
            default:
                break;
        }

    }


}
