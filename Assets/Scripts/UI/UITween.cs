using System.Collections;
using UnityEngine;

public class UITween : BaseSingleton<UITween>
{

    private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    /// <summary>
    /// UI元素的Move动画逻辑
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="startPos"></param>
    /// <param name="targgetPos"></param>
    /// <param name="transDuration"></param>
    public IEnumerator UIDoMove(Transform transform, Vector2 startPos, Vector2 targgetPos, float transDuration)
    {
        transform.gameObject.SetActive(false);

        RectTransform rectTransform = transform.GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogWarning("目标身上缺失RectTransform组件");
            yield break;
        }

        //设置开始位置
        rectTransform.anchoredPosition = startPos;
        transform.gameObject.SetActive(true);

        //lerp进行位置缓动
        yield  return UIDoMove(rectTransform, startPos, targgetPos, transDuration);
    }

    public IEnumerator UIDoFade(Transform transform, float startValue, float endValue, float duration)
    {
        CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogWarning("目标身上缺失CanvasGroup组件");
            yield break;
        }

        canvasGroup.alpha = startValue;
        yield return UIDoFade(canvasGroup, startValue, endValue, duration);
    }

    IEnumerator UIDoFade(CanvasGroup canvasGroup, float startValue, float endValue, float duration)
    {
        float timer = 0;
        while (timer <= duration)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(startValue, endValue, curve.Evaluate(timer / duration));
            yield return null;
        }
    }

    /// <summary>
    ///  UI元素的LocalMove动画逻辑
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="targgetPos"></param>
    /// <param name="transDuration"></param>
    public IEnumerator UIDoLocalMove(Transform transform, Vector3 direction, float transDuration)
    {
        transform.gameObject.SetActive(false);
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        Vector3 startPos = rectTransform.anchoredPosition;
        Vector3 targetPos = startPos + direction;
        transform.gameObject.SetActive(true);
        yield return UIDoMove(rectTransform, startPos, targetPos, transDuration);
    }

    IEnumerator UIDoMove(RectTransform rectTransform, Vector2 startPos, Vector2 targetPos, float transDuration)
    {
        float timer = 0;
        while (timer <= transDuration)
        {
            timer += Time.unscaledDeltaTime;
            rectTransform.anchoredPosition = Vector3.Slerp(startPos, targetPos, curve.Evaluate(timer / transDuration));
            yield return null;
        }
    }


    MonoManager monoManager;


    //动画预设
    ///////////////////////////////////////////////////////////
    ///

    Vector2 moveTargetPos;
    /// <summary>
    /// 面板从目标方向移动至屏幕中央【覆盖屏幕压暗】
    /// </summary>
    /// <param name="dir">从何方来</param>
    /// <param name="canvasGroup">面板透明度CanvasGroup</param>
    /// <param name="uiRoot">面板移动跟</param>
    /// <param name="transTime">动画时长</param>
    /// <returns></returns>
    public IEnumerator UIEaseInFrom(E_Dir dir, Transform canvasGroup, Transform uiRoot, float transTime)
    {
        //黑幕
        if (monoManager == null)
            monoManager = MonoManager.Instance;

        switch (dir)
        {
            case E_Dir.上:
                moveTargetPos = new Vector2(0, 2000);
                break;
            case E_Dir.下:
                moveTargetPos = new Vector2(0, -2000);
                break;
            case E_Dir.左:
                moveTargetPos = new Vector2(2000, 0);
                break;
            case E_Dir.右:
                moveTargetPos = new Vector2(-2000, 0);
                break;
            default:
                moveTargetPos = new Vector2(0, 2000);
                break;
        }
        monoManager.StartCoroutine(UIDoFade(canvasGroup, 0, 1, transTime / 2));
        yield return UIDoMove(uiRoot, moveTargetPos, Vector2.zero, transTime / 2);
    }


    /// <summary>
    /// 面板从目标方向移动至屏幕中央【覆盖屏幕压暗】
    /// </summary>
    /// <param name="dir">从何方来</param>
    /// <param name="canvasGroup">面板透明度CanvasGroup</param>
    /// <param name="uiRoot">面板移动跟</param>
    /// <param name="transTime">动画时长</param>
    /// <returns></returns>
    public IEnumerator UIEaseOutTo(E_Dir dir, Transform canvasGroup, Transform uiRoot, float transTime)
    {
        //黑幕
        if (monoManager == null)
            monoManager = MonoManager.Instance;

        switch (dir)
        {
            case E_Dir.上:
                moveTargetPos = new Vector2(0, 2000);
                break;
            case E_Dir.下:
                moveTargetPos = new Vector2(0, -2000);
                break;
            case E_Dir.左:
                moveTargetPos = new Vector2(2000, 0);
                break;
            case E_Dir.右:
                moveTargetPos = new Vector2(-2000, 0);
                break;
            default:
                moveTargetPos = new Vector2(0, 2000);
                break;
        }

        yield return UIDoMove(uiRoot, Vector2.zero, moveTargetPos, transTime / 2);
        yield return UIDoFade(canvasGroup, 1, 0, transTime / 2);
    }
}

public enum E_Dir
{
    上, 下, 左, 右
}
