using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : BasePanel
{
    [Header("游玩按钮")]
    public Button PlayButton;
    [Header("成就按钮")]
    public Button AchievementButton;
    [Header("设置按钮")]
    public Button SettingButton;
    [Header("鸣谢按钮")]
    public Button ThankYouButton;
    [Header("退出按钮")]
    public Button QuitButton;

    /// <summary>
    /// 游玩按钮
    /// </summary>
    void OnClickPlayButton()
    {
        uiManager.ShowPanel<GameProfilePanel>(null,true);
    }

    /// <summary>
    /// 成就面板
    /// </summary>
    void OnClickAchievementButtonButton()
    {
        uiManager.ShowPanel<AchievementsPanel>(null,true);
    }
    /// <summary>
    /// 设置面板
    /// </summary>
    void OnClickSettingButton() 
    {
        uiManager.ShowPanel<SettingPanel>(null,true);
    }
    /// <summary>
    /// 鸣谢面板
    /// </summary>
    void OnClickThankYouButton() 
    {
        uiManager.ShowPanel<ThankYouPanel>(null,true);
    }

    /// <summary>
    /// 退出游戏
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
        //yield return uiTweener.UIEaseOutTo(E_Dir.下, transform, UIRoot, transTime);
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
        //yield return uiTweener.UIEaseInFrom(E_Dir.下, transform, UIRoot, transTime);
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
