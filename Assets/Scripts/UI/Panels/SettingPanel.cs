using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [Header("����ӳ�䰴ť")]
    public Button ActionSettingButton;
    [Header("�������ð�ť")]
    public Button AudioSettingButton;

    [Header("�������˵���ť")]
    public Button ReturnToMenuButton;

    [Header("�����ð�ť")]
    public Button ShakeSettingButton;

    [Header("���ذ�ť")]
    public Button ReturnButton;

    /// <summary>
    /// �ص����˵���ť��
    /// </summary>
    void OnClickReturnToMenuButtton()
    {

    }

    /// <summary>
    ///  �����ð�ť��
    /// </summary>
    void OnClickIShakeSettingButton() => uiManager.ShowPanel<TechPanel>(null);

    /// <summary>
    /// ����ģʽ���ð�ť��
    /// </summary>
    void OnClickAcionSettingButton() => uiManager.ShowPanel<ActionSettingPanel>(null, true);
    /// <summary>
    /// ������Ч���ð�ť��
    /// </summary>
    void OnClickAudioSettingButton() => uiManager.ShowPanel<AudioPanel>(null, true);

    /// <summary>
    /// �������ð�ť��
    /// </summary>
    void OnClickReturnButton() => uiManager.HidePanel<SettingPanel>();

    /// <summary>
    /// �������˵����ð�ť��
    /// </summary>
    void OnClickReturnToMenuButton()
    {
        //�ص�persistent����
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
