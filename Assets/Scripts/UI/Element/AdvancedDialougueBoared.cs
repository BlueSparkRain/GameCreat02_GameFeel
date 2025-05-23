using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class AdvancedDialougueBoared : MonoBehaviour
{
    public Button triggerButton;
    [Header("�߼��ı�")]
    public AdvancedText displayText;
    [Header("�ı�������")]
    public TMP_Text speakerNameText;

    [Header("��ǰ���еĶԻ���")]
    public DialogueDataSequenceSO currentDialogueSq;

    Vector3 size;

    float oneLineLifeTime=3;
    UITween uiTween;

    private void Init()
    {
        uiTween ??= UITween.Instance;
        displayText ??=GetComponent<AdvancedText>();
        speakerNameText ??= GetComponent<AdvancedText>();
    }

    void OnClickTriggerButton() 
    {
        Debug.Log("Trigger����");

        ShowMyDialogue();
    }

    private void Awake()
    {
        Init();
    }

    IEnumerator AppearAnim(Vector3 size) 
    {
        //yield return uiTween.UIDoFade(transform, 0, 1, 1f);
        transform.GetChild(0).localScale=new Vector3(0f, 2.2f, 1f);
        yield return  TweenHelper.MakeLerp(transform.GetChild(0).localScale,new Vector3(1.8f,0.6f,1f),0.1f,val=>transform.GetChild(0).localScale =val) ;
        yield return  TweenHelper.MakeLerp(transform.GetChild(0).localScale,size,0.06f,val=>transform.GetChild(0).localScale =val) ;
    }

    IEnumerator StartDisplay(Vector3 size)
    {
        yield return AppearAnim(size);
        yield return new WaitForSeconds(0.5f);
        ShowMyDialogue();

    }


    public void StartNewDialogueSequence(float oneLineLifeTime, float disAppearDelay, Vector3 size, DialogueDataSequenceSO currentDialogueSq) 
    {
       this.oneLineLifeTime=oneLineLifeTime;
       this.currentDialogueSq = currentDialogueSq;

       nextLineDelay = new WaitForSeconds(oneLineLifeTime);
       this.disAppearDelay=new WaitForSeconds(disAppearDelay);
       currentDialogueSq.currentIndex = 0;
       StartCoroutine(StartDisplay(size));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="speaker">������</param>
    /// <param name="content">��ӡ����</param>
    /// <param name="LineLifeTime">��ʾʱ��</param>
    /// <param name="displayType">��ӡ��ʽ</param>
    /// <param name="needTypeWithFade">���ֻ�����</param>
    /// <param name="fadeDuration">���ּ��</param>
    void ShowMyDialogue()
    {
        if (displayText.text != "")
            displayText.TextDisAppear();
        
        speakerNameText.text = currentDialogueSq.dialogueLine[currentDialogueSq.currentIndex].speaker;
        StartCoroutine(displayText.ShowText(currentDialogueSq));

        StartCoroutine(WaitForNextLine());
    }

     WaitForSeconds nextLineDelay;
     WaitForSeconds disAppearDelay;

    IEnumerator WaitForNextLine() 
    {
      yield return nextLineDelay;
      NextLine();
    }


    void NextLine() 
    {
        currentDialogueSq.currentIndex++;

        if (currentDialogueSq.currentIndex > currentDialogueSq.dialogueLine.Count)
            ShowMyDialogue();
        else
        {
            //Debug.Log("ji1");
            StartCoroutine(DisAppear());
        }
    }

    IEnumerator DisAppear() 
    {
        yield return disAppearDelay;
        yield return uiTween.UIDoFade(transform,1,0,1f);
    
    }
}

