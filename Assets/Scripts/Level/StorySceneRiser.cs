using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.XR;

public class StorySceneRiser : MonoBehaviour
{
    public List<DialogueSceneSetting> dialogue;
    DialogueManager dialogueManager;
    DialogueSceneSetting  newDialogue;
    public Transform noiseScreen;
    public AnimationCurve noiseScreenCurve;


    void Start()
    {
        dialogueManager = DialogueManager.Instance;
        //UIManager.Instance.ShowPanel<BlackPanel>(null);
        StartCoroutine(ShowDialogues());

        StartCoroutine(UnLockLevelSelect());
    }

    IEnumerator UnLockLevelSelect() 
    {
        yield return new WaitForSeconds(2);
        ////��ʱ��ҿ��Խ���ؿ�ѡ�����
        //UIManager.Instance.HidePanel<BlackPanel>();
        yield return NoiseScreenOpen();
        EventCenter.Instance.EventTrigger(E_EventType.E_StoryOver);
    }

    IEnumerator NoiseScreenOpen() 
    {
        yield return TweenHelper.MakeLerp(new Vector3(1,1,0),Vector3.one,0.25f, val=> noiseScreen.localScale=val,noiseScreenCurve);
    }

    IEnumerator ShowDialogues()
    {

        for (int i = 0; i < dialogue.Count; i++) 
        {
            newDialogue=dialogue[i];
            dialogueManager.CreatNewUnInteractableDialogue(true,newDialogue.resID, newDialogue.trans, Vector3.one*newDialogue.scale);
            yield return new WaitForSeconds(dialogue[i].displayInterval);
        }

        //yield return new WaitForSeconds(2);
        //DialogueManager.Instance.CreatNewUnInteractableDialogue(1, new Vector2(100, 350), Vector3.one*1.5f);
        //yield return new WaitForSeconds(2.5f);
        //DialogueManager.Instance.CreatNewUnInteractableDialogue(2, new Vector2(250, -350), Vector3.one *2.0f);
        //yield return new WaitForSeconds(1);

        ////��ʱ��ҿ��Խ���ؿ�ѡ�����
        //EventCenter.Instance.EventTrigger(E_EventType.E_StoryOver);

        //DialogueManager.Instance.CreatNewUnInteractableDialogue(3, new Vector2(150, -150), Vector3.one*1.8f);
        //yield return new WaitForSeconds(2.5f);
    }
}

[Serializable]
public class DialogueSceneSetting 
{
    [Header("չʾ���")]
    public float displayInterval;
    [Header("SO����ID")]
    public int  resID;
    [Header("չʾλ��")]
    public Transform trans;
    [Header("���ű���")]
    public float scale;
}