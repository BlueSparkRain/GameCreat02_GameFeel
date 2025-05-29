using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [Header("输入映射按钮")]
    public Button ActionSettingButton;
    [Header("音量设置按钮")]
    public Button AudioSettingButton;

    [Header("退出本关按钮")]
    public Button ExitCurrentLevelButton;

    [Header("震动设置按钮")]
    public Button ShakeSettingButton;

    [Header("返回按钮")]
    public Button ReturnButton;

    /// <summary>
    /// 退出当前关卡按钮绑定
    /// </summary>
    void OnClickExitCurrentLevelButtton()
    {
        //返回关卡选择界面
        SceneLoadManager.Instance.TransToLoadScene(2,E_SceneTranType.过场图过渡);
    }

    /// <summary>
    ///  震动设置按钮绑定
    /// </summary>
    void OnClickIShakeSettingButton()
    {    
        //uiManager.ShowPanel<TechPanel>(null); 
    }

    /// <summary>
    /// 输入模式设置按钮绑定
    /// </summary>
    void OnClickAcionSettingButton()
    {
        Application.Quit();
        //uiManager.ShowPanel<ActionSettingPanel>(null, true);
    
    }
    /// <summary>
    /// 音乐音效设置按钮绑定
    /// </summary>
    void OnClickAudioSettingButton() => uiManager.ShowPanel<AudioPanel>(null,true);

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
    bool hasOpen;

    protected override void Init()
    {
        base.Init();
        ExitCurrentLevelButton.onClick.AddListener(OnClickExitCurrentLevelButtton);
        AudioSettingButton.onClick.AddListener(OnClickAudioSettingButton);
        ActionSettingButton.onClick.AddListener(OnClickAcionSettingButton);
        ShakeSettingButton.onClick.AddListener(OnClickIShakeSettingButton);
        ReturnButton.onClick.AddListener(OnClickReturnButton);
    }
    public override void ShowPanel()
    {
        if(!hasOpen)
        {
            hasOpen = true;
             base.ShowPanel();
            canTrigger = true;
            canTrans=false;
            StartCoroutine(uiTweener.UIEaseInFrom(E_Dir.下, transform, UIRoot, transTime));

        }
        //hasOpen=true;
    }
    bool canTrans;

    public override IEnumerator ShowPanelTweenEffect()
    {
        if (!canTrans)
        {
            yield return uiTweener.UIEaseInFrom(E_Dir.下, transform, UIRoot, transTime);
            canTrans = true;
        }
        //}
    }
    bool canTrigger;
    private void Update()
    {
        if (canTrans && canTrigger && Input.GetKeyDown(KeyCode.Escape)) 
        {
            canTrans = false;       
            canTrigger = false;
            uiManager.HidePanel<SettingPanel>();
        }
    }
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.下, transform, UIRoot, transTime);
        hasOpen=false;

    }
}
