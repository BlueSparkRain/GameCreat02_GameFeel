using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSettingPanel : BasePanel
{
    [Header("�������ģʽ")]
    public Dropdown inputTypeDropDown;

    void Start()
    {
        inputTypeDropDown.onValueChanged.AddListener(OnInputDropDownValueChange);
    }


    /// <summary>
    /// Ϊ�����˵������л��߼�
    /// </summary>
    /// <param name="val">����ֵ</param>
    void OnInputDropDownValueChange(int val) 
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
        yield return uiTweener.UIEaseInFrom(E_Dir.��, transform, UIRoot, transTime);
    }


    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.��, transform, UIRoot, transTime);
    }


}
