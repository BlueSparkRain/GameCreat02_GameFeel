using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [Header("����")]
    public Transform Root;

    [Header("�������˵���ť")]
    public Button ReturnToMenuButton;
    [Header("����ӳ�䰴ť")]
    public Button InputActionButton;
    [Header("�������ð�ť")]
    public Button AudioSettingButton;
    //[Header("�������˵���ť")]
    //public Button ReturnToMenuButton;
    [Header("���ذ�ť")]
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
