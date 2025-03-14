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
        //设置开始位置
        rectTransform.anchoredPosition = startPos;
        transform.gameObject.SetActive(true);

        //lerp进行位置缓动
        return UIDoMove(rectTransform, startPos, targgetPos, transDuration);
    }

    public IEnumerator UIDoFade(Transform transform, float startValue, float endValue, float duration)
    {
        CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();
        canvasGroup.alpha = startValue;
        return UIDoFade(canvasGroup, startValue, endValue, duration);
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
}
