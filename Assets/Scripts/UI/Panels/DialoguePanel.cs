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
    [Header("�߼��ı�")]
    public AdvancedText displayText;
    [Header("�ı�������")]
    public TMP_Text speakerNameText;

    //���Կ�����ʾ
    bool _canQuickShow;
    //�Զ���ʾ��һ��
    bool _canAutonNext;
    [Header("�ı���ӡ��Ԥ�Ƽ�")]
    public GameObject displayBoardPrefab;


    //public void ShowUnInteractableLine(Vector2 bornPos,float oneLineLifeTime, DialogueDataSequenceSO currentDialogueSq) 
    //{
    //    AdvancedDialougueBoared board=Instantiate(displayBoardPrefab,bornPos,Quaternion.identity,transform).GetComponent<AdvancedDialougueBoared>();
    //    board.transform.localPosition = bornPos;
    //    board.StartNewDialogueSequence(oneLineLifeTime,currentDialogueSq);

    //}


    /// <summary>
    /// ��ʾ(��Ҫ����)�Ի�
    /// </summary>
    /// <param name="speaker">������</param>
    /// <param name="content">����</param>
    /// <param name="needTyping">ʹ�ô��ֻ�Ч��</param>
    /// <param name="fadeDuration">��ӡ���</param>
    /// <param name="canQickShow">������ʾ</param>
    /// <param name="canAutonNext">�Զ���һ��</param>

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