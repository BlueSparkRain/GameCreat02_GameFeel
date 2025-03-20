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
    /// �����붯�������߼���ִ�еȼ�2
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator ShowPanelTweenEffect();

    /// <summary>
    /// �����ʾ���ã�ִ�еȼ�1
    /// </summary>
    public virtual void ShowPanel()
    {
        transform.SetAsLastSibling();
        Init();
        if (NeedPausePanel) 
        Time.timeScale = 0;
        
    }

    /// <summary>
    /// ����˳����ã�ִ�еȼ�1
    /// </summary>
    public virtual void HidePanel()
    {
        if (NeedPausePanel)
        Time.timeScale = 1;
    }
    /// <summary>
    /// ����˳����������߼���ִ�еȼ�2
    /// </summary>
    public abstract IEnumerator HidePanelTweenEffect();

    protected virtual void Init()
    { 
     if(FirstSelectButton)
            EventSystem.current.SetSelectedGameObject(FirstSelectButton);
    }

}
