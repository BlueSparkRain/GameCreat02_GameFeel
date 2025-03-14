using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : BasePanel
{
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return null;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return null;
    }

    protected override void Init()
    {
        base.Init();
    }
}
