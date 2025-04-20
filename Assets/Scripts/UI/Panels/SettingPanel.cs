using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [Header("输入映射按钮")]
    public Button ActionSettingButton;
    [Header("音量设置按钮")]
    public Button AudioSettingButton;

    [Header("返回主菜单按钮")]
    public Button ReturnToMenuButton;

    [Header("震动设置按钮")]
    public Button ShakeSettingButton;

    [Header("返回按钮")]
    public Button ReturnButton;

    /// <summary>
    /// 回到主菜单按钮绑定
    /// </summary>
    void OnClickReturnToMenuButtton()
    {

    }

    /// <summary>
    ///  震动设置按钮绑定
    /// </summary>
    void OnClickIShakeSettingButton() => uiManager.ShowPanel<TechPanel>(null);

    /// <summary>
    /// 输入模式设置按钮绑定
    /// </summary>
    void OnClickAcionSettingButton() => uiManager.ShowPanel<ActionSettingPanel>(null, true);
    /// <summary>
    /// 音乐音效设置按钮绑定
    /// </summary>
    void OnClickAudioSettingButton() => uiManager.ShowPanel<AudioPanel>(null, true);

    /// <summary>
    /// 返回设置按钮绑定
    /// </summary>
    void OnClickReturnButton() => uiManager.HidePanel<SettingPanel>();

    /// <summary>
    /// 返回主菜单设置按钮绑定
    /// </summary>
    void OnClickReturnToMenuButton()
    {
        //回到persistent场景
        uiManager.ShowPanel<MenuPanel>(null, true);
    }

    protected override void Init()
    {
        base.Init();
        ReturnToMenuButton.onClick.AddListener(OnClickReturnToMenuButtton);
        AudioSettingButton.onClick.AddListener(OnClickAudioSettingButton);
        ActionSettingButton.onClick.AddListener(OnClickAcionSettingButton);
        ShakeSettingButton.onClick.AddListener(OnClickIShakeSettingButton);
        ReturnButton.onClick.AddListener(OnClickReturnButton);
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
