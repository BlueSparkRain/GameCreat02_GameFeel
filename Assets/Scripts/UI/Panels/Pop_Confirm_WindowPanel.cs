using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 多功能确认弹窗【面板】
/// </summary>
public class Pop_Confirm_WindowPanel : BasePanel
{
    [Header("确认按钮")]
    public Button ConfirmButton;
    [Header("返回按钮")]
    public Button ReturnButton;
    [Header("确认文本")]
    public TMP_Text confirmText;


    public override void HidePanel()
    {
        base.HidePanel();
        ConfirmButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.RemoveAllListeners();  
    }

    public void ToConfirm( string str ,UnityAction confirmAction,UnityAction disposeAction) 
    {
        if (confirmAction != null)
        {
            confirmText.text = str;
            Debug.Log("我要进行确认！");
            ConfirmButton.onClick.AddListener(
                () => {
                    confirmAction?.Invoke();
                    uiManager.HidePanel<Pop_Confirm_WindowPanel>();
                });
            ReturnButton.onClick.AddListener(
                () =>
                {
                    disposeAction?.Invoke();
                    uiManager.HidePanel<Pop_Confirm_WindowPanel>();
                });
        }
        else 
        {
            confirmText.text = str;
            Debug.Log("无法确认！");
        }
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.下, transform, UIRoot, transTime);
    }

    public override void ShowPanel()
    {

        base.ShowPanel();
        
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
            yield return uiTweener.UIEaseInFrom(E_Dir.下, transform, UIRoot, transTime);
    }

    protected override void Init()
    {
        base.Init();
    }
}
