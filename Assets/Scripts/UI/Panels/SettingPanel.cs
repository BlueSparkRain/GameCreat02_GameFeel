using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [Header("����ӳ�䰴ť")]
    public Button ActionSettingButton;
    [Header("�������ð�ť")]
    public Button AudioSettingButton;

    [Header("�˳����ذ�ť")]
    public Button ExitCurrentLevelButton;

    [Header("�����ð�ť")]
    public Button ShakeSettingButton;

    [Header("���ذ�ť")]
    public Button ReturnButton;

    /// <summary>
    /// �˳���ǰ�ؿ���ť��
    /// </summary>
    void OnClickExitCurrentLevelButtton()
    {
        //���عؿ�ѡ�����
        SceneLoadManager.Instance.TransToLoadScene(2,E_SceneTranType.����ͼ����);
    }

    /// <summary>
    ///  �����ð�ť��
    /// </summary>
    void OnClickIShakeSettingButton()
    {    
        //uiManager.ShowPanel<TechPanel>(null); 
    }

    /// <summary>
    /// ����ģʽ���ð�ť��
    /// </summary>
    void OnClickAcionSettingButton()
    {
        Application.Quit();
        //uiManager.ShowPanel<ActionSettingPanel>(null, true);
    
    }
    /// <summary>
    /// ������Ч���ð�ť��
    /// </summary>
    void OnClickAudioSettingButton() => uiManager.ShowPanel<AudioPanel>(null,true);

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
            StartCoroutine(uiTweener.UIEaseInFrom(E_Dir.��, transform, UIRoot, transTime));

        }
        //hasOpen=true;
    }
    bool canTrans;

    public override IEnumerator ShowPanelTweenEffect()
    {
        if (!canTrans)
        {
            yield return uiTweener.UIEaseInFrom(E_Dir.��, transform, UIRoot, transTime);
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
        yield return uiTweener.UIEaseOutTo(E_Dir.��, transform, UIRoot, transTime);
        hasOpen=false;

    }
}
