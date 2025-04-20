using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSettingPanel : BasePanel
{
    [Header("玩家输入模式")]
    public Dropdown inputTypeDropDown;

    void Start()
    {
        inputTypeDropDown.onValueChanged.AddListener(OnInputDropDownValueChange);
    }


    /// <summary>
    /// 为下拉菜单配置切换逻辑
    /// </summary>
    /// <param name="val">更改值</param>
    void OnInputDropDownValueChange(int val) 
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

    protected override void Init()
    {
        base.Init();
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }


    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.下, transform, UIRoot, transTime);
    }


    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.下, transform, UIRoot, transTime);
    }


}
