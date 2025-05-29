using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPanel : BasePanel
{
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIDoFade(transform,1,0,transTime);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return new WaitForSeconds(1);
        yield return uiTweener.UIDoFade(transform,0,1,transTime);

    }

    protected override void Init()
    {
        base.Init();
    }
}
