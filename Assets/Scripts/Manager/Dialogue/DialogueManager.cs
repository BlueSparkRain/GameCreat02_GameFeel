using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    [Header("��ǰ���еĶԻ���")]
    public DialogueDataSequenceSO currentDialogueSq;
    DialoguePanel D_Panel;//�Ի����
    UIManager uIManager;

    GameObject displayBoardPrefab;

    /// <summary>
    /// ΪĿ��Ի������ί��
    /// </summary>
    /// <param name="ID">�Ի���ID</param>
    /// <param name="index">�¼�������λ������</param>
    /// <param name="action">�Ի�ί��</param>
    public void AddDialogueEvent(int ID, int index, Action action)
    {
        currentDialogueSq.eventList.Add(new DialogueEvent(index, action));
    }

    /// <summary>
    /// ����ǽ����Ͷ�����ʾ
    /// </summary>
    /// <param name="ID">"StaticData/DialogueLine/UnInteractable/"Ŀ¼�µ�ID</param>
    /// <param name="bornPos"></param>
    /// <param name="oneLineLifeTime"></param>
    /// <param name="action"></param>
    public void CreatNewUnInteractableDialogue(int ID,Vector2 bornPos,Vector3 size,float oneLineLifeTime=3 , float diaAppearDelay=4,Action action = null) 
    {
        var currentDialogueSq = Resources.Load<DialogueDataSequenceSO>("SOData/DialogueLineSO/UnInteractable/" + ID);
        action?.Invoke();//���նԻ�ί��
        ShowUnInteractableLine(bornPos, oneLineLifeTime, size , diaAppearDelay, currentDialogueSq);


    }

    void ShowUnInteractableLine(Vector2 bornPos, float oneLineLifeTime, Vector3 size,float disAppearDelay ,DialogueDataSequenceSO currentDialogueSq)
    {
        AdvancedDialougueBoared board = Instantiate(displayBoardPrefab, bornPos, Quaternion.identity, null).GetComponent<AdvancedDialougueBoared>();
        board. transform.GetChild(0). localPosition = bornPos;
        board. transform.GetChild(0).localScale= size;

        board.StartNewDialogueSequence(oneLineLifeTime, disAppearDelay,size,currentDialogueSq);

    }

    protected override void InitSelf()
    {
        base.InitSelf();
        uIManager = UIManager.Instance;
        displayBoardPrefab ??= Resources.Load<GameObject>("Prefab/UIPanel/UIElement/AdvancedDialogueBoardElement");
    }

    /// <summary>
    /// ����һ�οɽ����Ի�
    /// </summary>
    /// <param name="ID">StaticData/DialogueLine/Interactable/"Ŀ¼�µ�ID</param>
    /// <param name="action">�Ի�ί��</param>
    public void BeginInteractableDialogueSequence(int ID, Action action = null)
    {
        SetCurrentDialogueSquence(ID);
        action?.Invoke();//���նԻ�ί��
        uIManager.ShowPanel<DialoguePanel>(panel =>
        {
            panel.ShowInteractableDialogue(currentDialogueSq.dialogueLine[currentDialogueSq.currentIndex].speaker,
                                                                currentDialogueSq.dialogueLine[currentDialogueSq.currentIndex].content,
                                                                currentDialogueSq.displayType,currentDialogueSq.needTypeWithFade,currentDialogueSq.fadeDuration,
                                                                currentDialogueSq.canQuickShow, currentDialogueSq.canAutonNext);
            D_Panel = panel;
        });
        Debug.Log(currentDialogueSq.currentIndex);
        ActionCheck();
    }

    /// <summary>
    /// �������ص�ǰ�ı�����ʾ��һ���ı�
    /// </summary>
    public IEnumerator NextDialogue()
    {
        if (currentDialogueSq?.currentIndex + 1 < currentDialogueSq?.dialogueLine.Count)
        {
            currentDialogueSq.currentIndex++;
            D_Panel.ShowInteractableDialogue(currentDialogueSq.dialogueLine[currentDialogueSq.currentIndex].speaker,
                                                          currentDialogueSq.dialogueLine[currentDialogueSq.currentIndex].content,
                                                          currentDialogueSq.displayType, currentDialogueSq.needTypeWithFade, currentDialogueSq.fadeDuration,
                                                          currentDialogueSq.canQuickShow, currentDialogueSq.canAutonNext);
            Debug.Log(currentDialogueSq.currentIndex);
            ActionCheck();
        }
        else
        {
            EndDialogueSquence();//�����Ի�
        }
        yield return null;
    }


    /// <summary>
    /// ���õ�ǰ�Ի���
    /// </summary>
    /// <param name="ID">�Ի�����ID</param>
    void SetCurrentDialogueSquence(int ID)
    {
        currentDialogueSq = Resources.Load<DialogueDataSequenceSO>("SOData/DialogueLineSO/Interactable" + ID);
        currentDialogueSq.currentIndex = 0;
    }

    /// <summary>
    /// ÿ����һ��Ի�ʱ����Ƿ���ί����Ҫִ��
    /// </summary>
    void ActionCheck()
    {
        for (int i = 0; i < currentDialogueSq.eventList.Count; i++)
        {
            if (currentDialogueSq.eventList[i].eventIndex == currentDialogueSq.currentIndex)
            {
                currentDialogueSq.eventList[i].MyEvent?.Invoke();
                Debug.Log("��⵽�Ի��¼�������");
            }
        }
    }

    /// <summary>
    /// ����һ�ζԻ�����
    /// </summary>
    void EndDialogueSquence()
    {
        currentDialogueSq?.eventList.Clear();
        currentDialogueSq = null;
        UIManager.Instance.HidePanel<DialoguePanel>();
    }

    private void OnApplicationQuit()
    {
        currentDialogueSq?.eventList.Clear();
        currentDialogueSq = null;
    }
}
