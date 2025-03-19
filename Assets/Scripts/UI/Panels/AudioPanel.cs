using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPanel : BasePanel
{
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
    }
}
