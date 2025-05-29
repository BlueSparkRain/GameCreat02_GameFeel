using System.Collections;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public abstract class BasePanel : MonoBehaviour
{
    [Header("动画持续时间")]
    public float transTime = 1;
    [Header("手柄模式默认按钮")]
    public GameObject FirstSelectButton;
    [Header("UI移动根")]
    public Transform UIRoot;
    [Header("暂停时间")]
    public bool NeedPausePanel;

    public AnimationCurve AnimCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    protected UIManager uiManager;
    protected UITween uiTweener;
    protected GameInputModeManager gameInputModeManager;
    protected GameProfileSaveManager gameProfileSaveManager;

    protected bool initSelf;
    protected bool canClosePanel;

    public IEnumerator LockSelf()
    {
        if (!GetComponentInParent<CanvasGroup>())
            yield break;

        GetComponentInParent<CanvasGroup>().interactable = false;
        yield return new WaitForSeconds(2);
        GetComponentInParent<CanvasGroup>().interactable = true;
    }

    public void ClearSelf()
    {
        DestroyImmediate(gameObject);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    protected virtual void Init()
    {
        //预缓存管理器
        if (uiManager == null)
        {
            uiManager = UIManager.Instance;
            gameInputModeManager = GameInputModeManager.Instance;
            uiTweener = UITween.Instance;
            gameProfileSaveManager = GameProfileSaveManager.Instance;
        }

        //手柄模式下,分配默认按钮
        if (GameInputModeManager.Instance.inputType == E_Input.手柄)
        {

        }

        if (FirstSelectButton)
        {
            PlayerInputManager.Instance.SetCurrentSelectGameObj(FirstSelectButton);
        }
    }

    /// <summary>
    /// 面板显示调用：执行等级1
    /// </summary>
    public virtual void ShowPanel()
    {
        Init();
        transform.SetAsLastSibling();
        if (NeedPausePanel)
            Time.timeScale = 0;
    }

    /// <summary>
    /// 面板进入动画缓动逻辑：执行等级2
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator ShowPanelTweenEffect();

    /// <summary>
    /// 面板退出调用：执行等级1
    /// </summary>
    public virtual void HidePanel()
    {
        if (NeedPausePanel)
            Time.timeScale = 1;
    }

    /// <summary>
    /// 面板退出动画缓动逻辑：执行等级2
    /// </summary>
    public abstract IEnumerator HidePanelTweenEffect();
}
