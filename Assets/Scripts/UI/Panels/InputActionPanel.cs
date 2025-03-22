using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputActionPanel : BasePanel
{



    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime / 2);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 0, 1, transTime / 2);
    }

    protected override void Init()
    {
        base.Init();
    }
    public override void GamePadClose()
    {
        base.GamePadClose();
        UIManager.Instance.HidePanel<InputActionPanel>();
    }

}
