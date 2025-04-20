using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// �๦��ȷ�ϵ�������塿
/// </summary>
public class Pop_Confirm_WindowPanel : BasePanel
{
    [Header("ȷ�ϰ�ť")]
    public Button ConfirmButton;
    [Header("���ذ�ť")]
    public Button ReturnButton;
    [Header("ȷ���ı�")]
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
            Debug.Log("��Ҫ����ȷ�ϣ�");
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
            Debug.Log("�޷�ȷ�ϣ�");
        }
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.��, transform, UIRoot, transTime);
    }

    public override void ShowPanel()
    {

        base.ShowPanel();
        
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
            yield return uiTweener.UIEaseInFrom(E_Dir.��, transform, UIRoot, transTime);
    }

    protected override void Init()
    {
        base.Init();
    }
}
