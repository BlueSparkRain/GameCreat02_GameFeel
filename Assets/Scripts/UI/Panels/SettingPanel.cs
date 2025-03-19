using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [Header("返回主菜单按钮")]
    public Button ReturnToMenuButton;
    [Header("输入映射按钮")]
    public Button InputButton;
    [Header("音量设置按钮")]
    public Button AudioSettingButton;
    //[Header("返回主菜单按钮")]
    //public Button ReturnToMenuButton;
    //[Header("返回主菜单按钮")]
    //public Button ReturnToMenuButton;


    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return null;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return null;
    }

    protected override void Init()
    {
        base.Init();
    }
}
