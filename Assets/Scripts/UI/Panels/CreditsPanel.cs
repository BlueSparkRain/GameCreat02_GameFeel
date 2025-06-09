using System.Collections;
using UnityEngine.UI;

public class CreditsPanel : BasePanel
{
    public Button returnButton;

    public void OnClickReturnButton() 
    {
        uiManager.HidePanel<CreditsPanel>();
    
    }
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.ио, transform, UIRoot, transTime,0.8f);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.ио, transform, UIRoot, transTime,0.8f);
    }

    protected override void Init()
    {
        base.Init();
    }
}
