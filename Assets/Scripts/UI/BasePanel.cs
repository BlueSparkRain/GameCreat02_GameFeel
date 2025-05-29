using System.Collections;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public abstract class BasePanel : MonoBehaviour
{
    [Header("��������ʱ��")]
    public float transTime = 1;
    [Header("�ֱ�ģʽĬ�ϰ�ť")]
    public GameObject FirstSelectButton;
    [Header("UI�ƶ���")]
    public Transform UIRoot;
    [Header("��ͣʱ��")]
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
        //Ԥ���������
        if (uiManager == null)
        {
            uiManager = UIManager.Instance;
            gameInputModeManager = GameInputModeManager.Instance;
            uiTweener = UITween.Instance;
            gameProfileSaveManager = GameProfileSaveManager.Instance;
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

    /// <summary>
    /// �����ʾ���ã�ִ�еȼ�1
    /// </summary>
    public virtual void ShowPanel()
    {
        Init();
        transform.SetAsLastSibling();
        if (NeedPausePanel)
            Time.timeScale = 0;
    }

    /// <summary>
    /// �����붯�������߼���ִ�еȼ�2
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator ShowPanelTweenEffect();

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
}
