using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum E_DisplayType
{
    Defalut, Fading, Typing
}

public class DialoguePanel : BasePanel
{
    public Button nextButton;
    [Header("高级文本")]
    public AdvancedText displayText;
    [Header("文本讲述者")]
    public TMP_Text speakerNameText;

    //可以快速显示
    bool _canQuickShow;
    //自动显示下一句
    bool _canAutonNext;
    [Header("文本打印框预制件")]
    public GameObject displayBoardPrefab;


    //public void ShowUnInteractableLine(Vector2 bornPos,float oneLineLifeTime, DialogueDataSequenceSO currentDialogueSq) 
    //{
    //    AdvancedDialougueBoared board=Instantiate(displayBoardPrefab,bornPos,Quaternion.identity,transform).GetComponent<AdvancedDialougueBoared>();
    //    board.transform.localPosition = bornPos;
    //    board.StartNewDialogueSequence(oneLineLifeTime,currentDialogueSq);

    //}


    /// <summary>
    /// 显示(需要交互)对话
    /// </summary>
    /// <param name="speaker">讲述者</param>
    /// <param name="content">内容</param>
    /// <param name="needTyping">使用打字机效果</param>
    /// <param name="fadeDuration">打印间隔</param>
    /// <param name="canQickShow">快速显示</param>
    /// <param name="canAutonNext">自动下一句</param>

    public void ShowInteractableDialogue(string speaker, string content, E_DisplayType displayType=E_DisplayType.Typing, bool needTypeWithFade=true,float fadeDuration = 0.2f, bool canQickShow = true, bool canAutonNext = false)
    {
        speakerNameText.text = speaker;
        speakerNameText.GetComponent<Animator>().SetTrigger("Show");

        if (displayText.text != "")
            displayText.TextDisAppear();

        _canQuickShow = canQickShow;
        _canAutonNext = canAutonNext;

        StartCoroutine(displayText.ShowText(content, displayType, needTypeWithFade,fadeDuration));
    }

    void OnClickNextButton()
    {
        if (displayText.typingCor != null)
            displayText.TextQuickShow();
        else
            StartCoroutine(DialogueManager.Instance.NextDialogue());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (displayText.typingCor != null)
                displayText.TextQuickShow();
            else
                StartCoroutine(DialogueManager.Instance.NextDialogue());
        }
    }

    public override void HidePanel()
    {
        base.HidePanel();
        displayText.TextDisAppear();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime);
        //yield return new WaitForSeconds(.5f);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 0, 1, transTime);
    }

    protected override void Init()
    {
        base.Init();
        nextButton.onClick.AddListener(OnClickNextButton);
      
    }
}