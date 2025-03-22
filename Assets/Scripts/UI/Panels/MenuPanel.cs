using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanel : BasePanel
{
    [Header("���水ť")]
    public Button PlayButton;
    [Header("ͼ����ť")]
    public Button BookButton;
    [Header("���ð�ť")]
    public Button SettingButton;
    [Header("��л��ť")]
    public Button ThankYouButton;
    [Header("�˳���ť")]
    public Button QuitButton;


    void OnClickPlayButton() { 
        UIManager.Instance.HidePanel<MenuPanel>();
        SceneManager.LoadScene(1);
    }

    void OnClickQuitButton() 
    {
        Application.Quit();
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime / 2);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 0, 1, transTime / 2);
    }

    protected override void Init()
    {
        base.Init();
        PlayButton.onClick.AddListener(OnClickPlayButton);
        QuitButton.onClick.AddListener(OnClickQuitButton);
    }

    public override void GamePadClose()
    {
        base.GamePadClose();
        //UIManager.Instance.HidePanel<MenuPanel>();
    }
}
