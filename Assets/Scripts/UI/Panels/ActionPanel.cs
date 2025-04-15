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
    /// Ϊ�����˵������л��߼�
    /// </summary>
    /// <param name="val">����ֵ</param>
    void OnDropDownValueChange(int val) 
    {
        switch (val)
        {
            case 0:
                GameInputModeManager.Instance.SwitchInputType(E_Input.����);
                break;
            case 1:
                GameInputModeManager.Instance.SwitchInputType(E_Input.�ֱ�);
                break;
            default:
                break;
        }

    }


}
