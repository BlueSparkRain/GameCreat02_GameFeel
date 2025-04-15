using System.Collections;
using UnityEditor.Build.Content;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public abstract class BasePanel : MonoBehaviour
{
    [Header("动画持续时间")]
    public float transTime = 1;
    [Header("手柄模式默认按钮")]
    public GameObject FirstSelectButton;
    [Header("暂停时间")]
    public bool NeedPausePanel;

    protected UIManager uiManager;

    protected GameInputModeManager gameInputModeManager;

    public virtual void GamePadClose()
    {


    }


    public virtual void InitGamePadMap()
    {
        //此处为场景中的EventSystem分配选中按钮
    }

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


    /// <summary>
    /// 面板进入动画缓动逻辑：执行等级2
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator ShowPanelTweenEffect();

    /// <summary>
    /// 面板显示调用：执行等级1
    /// </summary>
    public virtual void ShowPanel()
    {
        transform.SetAsLastSibling();
        Init();
        if (NeedPausePanel)
            Time.timeScale = 0;
    }


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
    public virtual IEnumerator HidePanelTweenEffect()
    {
        yield return null;
    }
    protected virtual void Init()
    {
        if (uiManager == null)
        {
            uiManager = UIManager.Instance;
            gameInputModeManager=GameInputModeManager.Instance;
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
}
