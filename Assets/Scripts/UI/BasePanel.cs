using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(CanvasGroup))]
public abstract class BasePanel : MonoBehaviour
{

    //private void OnEnable()
    //{
    //    EventCenter.Instance.AddEventListener(E_EventType.E_UIClose, GamePadClose);
    //}
    //private void OnDisable()
    //{
    //    EventCenter.Instance.RemoveEventListener(E_EventType.E_UIClose, GamePadClose);
    //}

    public virtual void GamePadClose() { }


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

        //IDestroy();
    }
    /// <summary>
    /// ����˳����������߼���ִ�еȼ�2
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
