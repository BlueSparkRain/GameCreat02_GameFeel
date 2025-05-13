using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : BasePanel
{
    [Header("���水ť")]
    public Button PlayButton;
    [Header("�ɾͰ�ť")]
    public Button AchievementButton;
    [Header("���ð�ť")]
    public Button SettingButton;
    [Header("��л��ť")]
    public Button ThankYouButton;
    [Header("�˳���ť")]
    public Button QuitButton;

    /// <summary>
    /// ���水ť
    /// </summary>
    void OnClickPlayButton()
    {
        uiManager.ShowPanel<GameProfilePanel>(null,true);
    }

    /// <summary>
    /// �ɾ����
    /// </summary>
    void OnClickAchievementButtonButton()
    {
        uiManager.ShowPanel<AchievementsPanel>(null,true);
    }
    /// <summary>
    /// �������
    /// </summary>
    void OnClickSettingButton() 
    {
        uiManager.ShowPanel<SettingPanel>(null,true);
    }
    /// <summary>
    /// ��л���
    /// </summary>
    void OnClickThankYouButton() 
    {
        uiManager.ShowPanel<ThankYouPanel>(null,true);
    }

    /// <summary>
    /// �˳���Ϸ
    /// </summary>
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
        //yield return uiTweener.UIEaseOutTo(E_Dir.��, transform, UIRoot, transTime);
        yield return uiTweener.UIDoScale(false,UIRoot,transTime,AnimCurve);
        canClosePanel = false;
        //GetComponent<CanvasGroup>().interactable = false;

    }


    public override void ShowPanel()
    {
        if (canClosePanel)
           return;
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        if(canClosePanel)
            yield break;

        yield return uiTweener.UIDoScale(true,UIRoot,transTime,AnimCurve);
        //yield return uiTweener.UIEaseInFrom(E_Dir.��, transform, UIRoot, transTime);
        canClosePanel = true;
    }
    bool init;

    protected override void Init()
    {
        base.Init();

        if (!init)
        {
            init = true;
            PlayButton.onClick.AddListener(OnClickPlayButton);
            ThankYouButton.onClick.AddListener(OnClickThankYouButton);
            AchievementButton.onClick.AddListener(OnClickAchievementButtonButton);
            SettingButton.onClick.AddListener(OnClickSettingButton);
            QuitButton.onClick.AddListener(OnClickQuitButton);
        }
    }
}
