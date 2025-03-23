using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanel : BasePanel
{
    [Header("游玩按钮")]
    public Button PlayButton;
    [Header("图鉴按钮")]
    public Button BookButton;
    [Header("设置按钮")]
    public Button SettingButton;
    [Header("鸣谢按钮")]
    public Button ThankYouButton;
    [Header("退出按钮")]
    public Button QuitButton;

    [Header("面板根")]
    public Transform root;

    void OnClickPlayButton() {
        UIManager.Instance.ShowPanel<SceneTransPanel>(panel => panel.SceneLoadingTrans(1));
        UIManager.Instance.HidePanel<MenuPanel>();
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
        yield return UITween.Instance.UIDoMove(root, Vector2.zero, new Vector2(0,-2000),transTime/3);
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime / 2);
    }


    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 0, 1, transTime / 3);
        yield return UITween.Instance.UIDoMove(root,new Vector2(0,-2000),Vector2.zero,transTime/2);
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
