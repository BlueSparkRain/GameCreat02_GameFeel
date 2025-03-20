using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [Header("面板根")]
    public Transform Root;

    [Header("返回主菜单按钮")]
    public Button ReturnToMenuButton;
    [Header("输入映射按钮")]
    public Button InputActionButton;
    [Header("音量设置按钮")]
    public Button AudioSettingButton;
    //[Header("返回主菜单按钮")]
    //public Button ReturnToMenuButton;
    [Header("返回按钮")]
    public Button ReturnButton;

    void OnClickReturnToMenuButtton() => UIManager.Instance.ShowPanel<MenuPanel>(null);
    void OnClickInputActionButton() => UIManager.Instance.ShowPanel<InputActionPanel>(null);
    void OnClickAudioSettingButton() => UIManager.Instance.ShowPanel<AudioPanel>(null);
    void OnClickReturnButton() => UIManager.Instance.HidePanel<SettingPanel>();

    protected override void Init()
    {
        base.Init();
        ReturnToMenuButton.onClick.AddListener(OnClickReturnToMenuButtton);
        AudioSettingButton.onClick.AddListener(OnClickAudioSettingButton);
        InputActionButton.onClick.AddListener(OnClickInputActionButton);
        ReturnButton.onClick.AddListener(OnClickReturnButton);
    }


    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime / 2);
        yield return UITween.Instance.UIDoMove(Root, Vector2.zero, new Vector2(0,-2000),transTime/2);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 0, 1, transTime / 2);
        yield return UITween.Instance.UIDoMove(Root,new Vector2(0,-2000),Vector2.zero,transTime/2);
    }
}
