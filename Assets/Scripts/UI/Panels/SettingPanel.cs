using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [Header("����")]
    public Transform Root;

    [Header("�������˵���ť")]
    public Button ReturnToMenuButton;
    [Header("����ӳ�䰴ť")]
    public Button TechButton;
    [Header("�������ð�ť")]
    public Button AudioSettingButton;
    //[Header("�������˵���ť")]
    //public Button ReturnToMenuButton;
    [Header("���ذ�ť")]
    public Button ReturnButton;

    void OnClickReturnToMenuButtton() 
    {
        UIManager.Instance.HidePanel<SettingPanel>();

             UIManager.Instance.ShowPanel<SceneTransPanel>(panel =>
             {
                 panel.SceneLoadingTrans(0);
                 DestroyImmediate(LevelSelectManager.Instance.gameObject);
             });

             //PlayerInputManager.Instance.SetCurrentSelectGameObj(MenuButton.gameObject);

             StartCoroutine(ClearAllPanels());
         

    }

    IEnumerator ClearAllPanels()
    {
        yield return new WaitForSeconds(3);
        UIManager.Instance.DestoryAllPanels();
    }
    void OnClickInputActionButton() => UIManager.Instance.ShowPanel<TechPanel>(null);
    void OnClickAudioSettingButton() => UIManager.Instance.ShowPanel<AudioPanel>(null);
    void OnClickReturnButton() => UIManager.Instance.HidePanel<SettingPanel>();

    protected override void Init()
    {
        base.Init();
        ReturnToMenuButton.onClick.AddListener(OnClickReturnToMenuButtton);
        AudioSettingButton.onClick.AddListener(OnClickAudioSettingButton);
        TechButton.onClick.AddListener(OnClickInputActionButton);
        ReturnButton.onClick.AddListener(OnClickReturnButton);
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoMove(Root, Vector2.zero, new Vector2(0,-2000),transTime/2);
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime / 2);
        yield return base.HidePanelTweenEffect();
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

    public override void GamePadClose()
    {
        base.GamePadClose();
        UIManager.Instance.HidePanel<SettingPanel>();
        PlayerInputManager.Instance.SettingMenuChange();
    }

}
