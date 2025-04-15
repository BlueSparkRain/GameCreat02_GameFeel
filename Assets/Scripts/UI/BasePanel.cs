using System.Collections;
using UnityEditor.Build.Content;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public abstract class BasePanel : MonoBehaviour
{
    [Header("��������ʱ��")]
    public float transTime = 1;
    [Header("�ֱ�ģʽĬ�ϰ�ť")]
    public GameObject FirstSelectButton;
    [Header("��ͣʱ��")]
    public bool NeedPausePanel;

    protected UIManager uiManager;

    protected GameInputModeManager gameInputModeManager;

    public virtual void GamePadClose()
    {


    }


    public virtual void InitGamePadMap()
    {
        //�˴�Ϊ�����е�EventSystem����ѡ�а�ť
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

        //�ֱ�ģʽ��,����Ĭ�ϰ�ť
        if (GameInputModeManager.Instance.inputType == E_Input.�ֱ�) 
        {
             
        }

        if (FirstSelectButton)
        {
            PlayerInputManager.Instance.SetCurrentSelectGameObj(FirstSelectButton);
        }
    }
}
