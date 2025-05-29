using System;
using System.Collections;
using UnityEngine;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    [Header("当前进行的对话线")]
    public DialogueDataSequenceSO currentDialogueSq;
    DialoguePanel D_Panel;//对话面板
    UIManager uIManager;

    GameObject displayBoardPrefab_UI;
    GameObject displayBoardPrefab_3D;

    /// <summary>
    /// 为目标对话线添加委托
    /// </summary>
    /// <param name="ID">对话线ID</param>
    /// <param name="index">事件触发的位置索引</param>
    /// <param name="action">对话委托</param>
    public void AddDialogueEvent(int ID, int index, Action action)
    {
        currentDialogueSq.eventList.Add(new DialogueEvent(index, action));
    }

    /// <summary>
    /// 创造非交互型独白显示
    /// </summary>
    /// <param name="ID">"StaticData/DialogueLine/UnInteractable/"目录下的ID</param>
    /// <param name="bornTran"></param>
    /// <param name="oneLineLifeTime"></param>
    /// <param name="action"></param>
    public void CreatNewUnInteractableDialogue(bool is3D, int ID, Transform bornTran, Vector3 size, float oneLineLifeTime = 3, float diaAppearDelay = 4, Action action = null)
    {
        var currentDialogueSq = Resources.Load<DialogueDataSequenceSO>("SOData/DialogueLineSO/UnInteractable/" + ID);
        action?.Invoke();//接收对话委托
        ShowUnInteractableLine(is3D, bornTran, oneLineLifeTime, size, diaAppearDelay, currentDialogueSq);


    }

    void ShowUnInteractableLine(bool is3D, Transform bornPos, float oneLineLifeTime, Vector3 size, float disAppearDelay, DialogueDataSequenceSO currentDialogueSq)
    {
        AdvancedDialougueBoared board = null;
        if (is3D)
            board = Instantiate(displayBoardPrefab_3D, bornPos.position, bornPos.rotation, null).GetComponent<AdvancedDialougueBoared>();
        else
            board = Instantiate(displayBoardPrefab_UI, bornPos.position, Quaternion.identity, null).GetComponent<AdvancedDialougueBoared>();
        board.transform.GetChild(0).localPosition = bornPos.position;
        board.transform.GetChild(0).localScale = size;

        board.StartNewDialogueSequence(oneLineLifeTime, disAppearDelay, size, currentDialogueSq);

    }

    protected override void InitSelf()
    {
        base.InitSelf();
        uIManager = UIManager.Instance;
        displayBoardPrefab_UI ??= Resources.Load<GameObject>("Prefab/UIPanel/UIElement/AdvancedDialogueBoardElement");
        displayBoardPrefab_3D ??= Resources.Load<GameObject>("Prefab/UIPanel/UIElement/3D_AdvancedDialogueBoardElement");

    }

    /// <summary>
    /// 开启一段可交互对话
    /// </summary>
    /// <param name="ID">StaticData/DialogueLine/Interactable/"目录下的ID</param>
    /// <param name="action">对话委托</param>
    public void BeginInteractableDialogueSequence(int ID, Action action = null)
    {
        SetCurrentDialogueSquence(ID);
        action?.Invoke();//接收对话委托
        uIManager.ShowPanel<DialoguePanel>(panel =>
        {
            panel.ShowInteractableDialogue(currentDialogueSq.dialogueLine[currentDialogueSq.currentIndex].speaker,
                                                                currentDialogueSq.dialogueLine[currentDialogueSq.currentIndex].content,
                                                                currentDialogueSq.displayType, currentDialogueSq.needTypeWithFade,
                                                                currentDialogueSq.needTypeWithScale,
                                                                currentDialogueSq.fadeDuration,
                                                                currentDialogueSq.scaleDuration,
                                                                currentDialogueSq.canQuickShow, currentDialogueSq.canAutonNext);
            D_Panel = panel;
        });
        Debug.Log(currentDialogueSq.currentIndex);
        ActionCheck();
    }

    /// <summary>
    /// 用于隐藏当前文本并显示下一句文本
    /// </summary>
    public IEnumerator NextDialogue()
    {
        if (currentDialogueSq?.currentIndex + 1 < currentDialogueSq?.dialogueLine.Count)
        {
            currentDialogueSq.currentIndex++;
            D_Panel.ShowInteractableDialogue(currentDialogueSq.dialogueLine[currentDialogueSq.currentIndex].speaker,
                                                          currentDialogueSq.dialogueLine[currentDialogueSq.currentIndex].content,
                                                          currentDialogueSq.displayType, currentDialogueSq.needTypeWithFade,
                                                          currentDialogueSq.needTypeWithScale,
                                                          currentDialogueSq.fadeDuration,
                                                          currentDialogueSq.scaleDuration,
                                                          currentDialogueSq.canQuickShow, currentDialogueSq.canAutonNext);
            Debug.Log(currentDialogueSq.currentIndex);
            ActionCheck();
        }
        else
        {
            EndDialogueSquence();//结束对话
        }
        yield return null;
    }


    /// <summary>
    /// 设置当前对话线
    /// </summary>
    /// <param name="ID">对话序列ID</param>
    void SetCurrentDialogueSquence(int ID)
    {
        currentDialogueSq = Resources.Load<DialogueDataSequenceSO>("SOData/DialogueLineSO/Interactable" + ID);
        currentDialogueSq.currentIndex = 0;
    }

    /// <summary>
    /// 每结束一句对话时检查是否有委托需要执行
    /// </summary>
    void ActionCheck()
    {
        for (int i = 0; i < currentDialogueSq.eventList.Count; i++)
        {
            if (currentDialogueSq.eventList[i].eventIndex == currentDialogueSq.currentIndex)
            {
                currentDialogueSq.eventList[i].MyEvent?.Invoke();
                Debug.Log("检测到对话事件触发！");
            }
        }
    }

    /// <summary>
    /// 结束一段对话序列
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
