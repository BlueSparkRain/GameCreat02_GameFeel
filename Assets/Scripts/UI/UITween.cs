using System.Collections;
using UnityEngine;

public class UITween : BaseSingleton<UITween>
{

    private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    /// <summary>
    /// UIԪ�ص�Move�����߼�
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
            Debug.LogWarning("Ŀ������ȱʧRectTransform���");
            yield break;
        }

        //���ÿ�ʼλ��
        rectTransform.anchoredPosition = startPos;
        transform.gameObject.SetActive(true);

        //lerp����λ�û���
        yield  return UIDoMove(rectTransform, startPos, targgetPos, transDuration);
    }

    public IEnumerator UIDoFade(Transform transform, float startValue, float endValue, float duration)
    {
        CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogWarning("Ŀ������ȱʧCanvasGroup���");
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
    ///  UIԪ�ص�LocalMove�����߼�
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


    //����Ԥ��
    ///////////////////////////////////////////////////////////
    ///

    Vector2 moveTargetPos;
    /// <summary>
    /// ����Ŀ�귽���ƶ�����Ļ���롾������Ļѹ����
    /// </summary>
    /// <param name="dir">�Ӻη���</param>
    /// <param name="canvasGroup">���͸����CanvasGroup</param>
    /// <param name="uiRoot">����ƶ���</param>
    /// <param name="transTime">����ʱ��</param>
    /// <returns></returns>
    public IEnumerator UIEaseInFrom(E_Dir dir, Transform canvasGroup, Transform uiRoot, float transTime)
    {
        //��Ļ
        if (monoManager == null)
            monoManager = MonoManager.Instance;

        switch (dir)
        {
            case E_Dir.��:
                moveTargetPos = new Vector2(0, 2000);
                break;
            case E_Dir.��:
                moveTargetPos = new Vector2(0, -2000);
                break;
            case E_Dir.��:
                moveTargetPos = new Vector2(2000, 0);
                break;
            case E_Dir.��:
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
    /// ����Ŀ�귽���ƶ�����Ļ���롾������Ļѹ����
    /// </summary>
    /// <param name="dir">�Ӻη���</param>
    /// <param name="canvasGroup">���͸����CanvasGroup</param>
    /// <param name="uiRoot">����ƶ���</param>
    /// <param name="transTime">����ʱ��</param>
    /// <returns></returns>
    public IEnumerator UIEaseOutTo(E_Dir dir, Transform canvasGroup, Transform uiRoot, float transTime)
    {
        //��Ļ
        if (monoManager == null)
            monoManager = MonoManager.Instance;

        switch (dir)
        {
            case E_Dir.��:
                moveTargetPos = new Vector2(0, 2000);
                break;
            case E_Dir.��:
                moveTargetPos = new Vector2(0, -2000);
                break;
            case E_Dir.��:
                moveTargetPos = new Vector2(2000, 0);
                break;
            case E_Dir.��:
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
    ��, ��, ��, ��
}
