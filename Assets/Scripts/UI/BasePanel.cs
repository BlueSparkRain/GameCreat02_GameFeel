using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(CanvasGroup))]
public abstract class BasePanel : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public float transTime = 1;
    public GameObject FirstSelectButton;
    public bool NeedPausePanel;

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
    public abstract IEnumerator HidePanelTweenEffect();

    protected virtual void Init()
    { 
     if(FirstSelectButton)
            EventSystem.current.SetSelectedGameObject(FirstSelectButton);
    }

}
