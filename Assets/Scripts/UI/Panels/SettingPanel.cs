using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [Header("�������˵���ť")]
    public Button ReturnToMenuButton;
    [Header("����ӳ�䰴ť")]
    public Button InputButton;
    [Header("�������ð�ť")]
    public Button AudioSettingButton;
    //[Header("�������˵���ť")]
    //public Button ReturnToMenuButton;
    //[Header("�������˵���ť")]
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
