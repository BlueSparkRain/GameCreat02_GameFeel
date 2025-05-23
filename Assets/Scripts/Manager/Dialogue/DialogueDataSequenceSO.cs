using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyCustomData/DialogueDataLineSO", fileName = "DialogueDataLine")]
public class DialogueDataSequenceSO : ScriptableObject
{
    [Header("对话线ID")]
    public int ID;
    [Header("自动下一句")]
    public bool canAutonNext;
    [Header("可以快进")]
    public bool canQuickShow;
    [Header("展示效果")]
    public E_DisplayType displayType;
    [Header("打字机渐显效果")]
    public bool needTypeWithFade=true;
    [Header("当前话语序号")]
    public int currentIndex;
    [Header("淡入时间")]
    public float fadeDuration;
    [Header("事件线")]
    public List<DialogueEvent> eventList = new List<DialogueEvent>();
    [Header("对话线")]
    public List<DialogueData> dialogueLine = new List<DialogueData>();
}
[Serializable]
public class DialogueEvent
{
    [Header("EventIndex")]
    public int eventIndex;
    [Header("UnityEvent")]
    public Action MyEvent;

    public DialogueEvent(int _eventIndex, Action _action)
    {
        eventIndex = _eventIndex;
        MyEvent = _action;
    }
}

[Serializable]
public class DialogueData
{
    [Header("讲述者")]
    public string speaker;
    [Multiline]
    [Header("内容文本")]
    public string content;
}
