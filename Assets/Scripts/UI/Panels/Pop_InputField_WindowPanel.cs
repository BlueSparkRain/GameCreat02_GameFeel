using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// �๦�����뵯������塿
/// </summary>
public class Pop_InputField_WindowPanel : BasePanel
{
    [Header("�û������ı����")]
    public TMP_InputField InputField;

    [Header("���밴ť")]
    public Button EnterButton;

    bool init;

    public override void HidePanel()
    {
        base.HidePanel();

        InputField.onValueChanged.RemoveAllListeners();
        EnterButton.onClick.RemoveAllListeners();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
            yield return uiTweener.UIEaseOutTo(E_Dir.��, transform, UIRoot, transTime);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        InputField.text= string.Empty;
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.��, transform, UIRoot, transTime);
    }

    public void InputToConfirm(UnityAction<string> inputOnValueAction, UnityAction confirmAnaction)
    {
        if (confirmAnaction != null)
        {
            InputField.onValueChanged.AddListener(

            (str) =>
            {
                inputOnValueAction?.Invoke(str);
            } );

            EnterButton.onClick.AddListener(()=>
            { 
                confirmAnaction?.Invoke();
                uiManager.HidePanel<Pop_InputField_WindowPanel>();
            });  
        }
    }

    protected override void Init()
    {
        base.Init();
    }
}
