using System.Collections;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public abstract class BasePanel : MonoBehaviour
{
    public virtual void GamePadClose() { }

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

        //IDestroy();
    }
    /// <summary>
    /// 面板退出动画缓动逻辑：执行等级2
    /// </summary>
    public virtual IEnumerator HidePanelTweenEffect()
    {
        yield return null;
        IDestroy();
    }

    protected virtual void Init()
    {
        //GetComponent<CanvasGroup>().interactable=true;
        if (FirstSelectButton)
        {
            //EventSystem.current.SetSelectedGameObject(FirstSelectButton);
            PlayerInputManager.Instance.SetCurrentSelectGameObj(FirstSelectButton);
        }
    }

    protected virtual void IDestroy()
    {
        //GetComponent<CanvasGroup>().interactable = false;
        PlayerInputManager.Instance.LostCurrentSelectGameObj();
        //EventSystem.current.SetSelectedGameObject(null);
    }
}
