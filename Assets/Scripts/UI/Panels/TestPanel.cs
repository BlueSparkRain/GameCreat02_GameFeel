using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanel : BasePanel
{
    public Transform root;
    public override void GamePadClose()
    {
        base.GamePadClose();
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {

        yield return UITween.Instance.UIDoMove(root, Vector2.zero, new Vector2(0, -2000), transTime / 3);
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime / 2);
        GetComponent<CanvasGroup>().interactable = false;
        yield  return base.HidePanelTweenEffect();
    }

    public override void InitGamePadMap()
    {
        base.InitGamePadMap();
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 0, 1, transTime / 3);
        yield return UITween.Instance.UIDoMove(root, new Vector2(0, -2000), Vector2.zero, transTime / 2);
    }

    protected override void Init()
    {
        base.Init();
    }
}
