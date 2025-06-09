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
        MusicManager.Instance.StopBKMusic();
        dialogueManager = DialogueManager.Instance;
        StartCoroutine(ShowDialogues());

        StartCoroutine(UnLockLevelSelect());
    }

    IEnumerator UnLockLevelSelect() 
    {
        yield return new WaitForSeconds(2);
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
    }
}

[Serializable]
public class DialogueSceneSetting 
{
    [Header("展示间隔")]
    public float displayInterval;
    [Header("SO数据ID")]
    public int  resID;
    [Header("展示位置")]
    public Transform trans;
    [Header("缩放倍数")]
    public float scale;
}