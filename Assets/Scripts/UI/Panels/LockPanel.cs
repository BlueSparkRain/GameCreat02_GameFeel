using System.Collections;
using UnityEngine;

/// <summary>
/// 未解锁面板
/// </summary>
public class LockPanel : BasePanel
{
    bool isAppear;
    WaitForSeconds delay = new WaitForSeconds(1.0f);
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        if (!isAppear)
            yield break;
        StartCoroutine(ShakeRotAnim());
        yield return uiTweener.UIEaseOutTo(E_Dir.上, transform, UIRoot, transTime);
        isAppear = false;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        if (isAppear)
            yield break;
        isAppear = true;

        yield return uiTweener.UIEaseInFrom(E_Dir.上, transform, UIRoot, transTime);
        StartCoroutine(ShakeRotAnim());

        StartCoroutine(AppearAWhile());
    }
    IEnumerator AppearAWhile()
    {
        yield return delay;
        uiManager.HidePanel<LockPanel>();
    }
    IEnumerator ShakeRotAnim()
    {
        yield return TweenHelper.MakeLerp(Vector3.zero, new Vector3(0, 0, 5.8f), 0.08f, val => UIRoot.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, -5.8f), new Vector3(0, 0, 5.8f), 0.08f, val => UIRoot.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, 5.8f), Vector3.zero, 0.08f, val => UIRoot.eulerAngles = val);
    }

    protected override void Init()
    {
        base.Init();
    }
}
