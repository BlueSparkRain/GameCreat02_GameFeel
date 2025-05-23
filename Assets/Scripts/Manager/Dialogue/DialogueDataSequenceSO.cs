using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyCustomData/DialogueDataLineSO", fileName = "DialogueDataLine")]
public class DialogueDataSequenceSO : ScriptableObject
{
    [Header("�Ի���ID")]
    public int ID;
    [Header("�Զ���һ��")]
    public bool canAutonNext;
    [Header("���Կ��")]
    public bool canQuickShow;
    [Header("չʾЧ��")]
    public E_DisplayType displayType;
    [Header("���ֻ�����Ч��")]
    public bool needTypeWithFade=true;
    [Header("��ǰ�������")]
    public int currentIndex;
    [Header("����ʱ��")]
    public float fadeDuration;
    [Header("�¼���")]
    public List<DialogueEvent> eventList = new List<DialogueEvent>();
    [Header("�Ի���")]
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
    [Header("������")]
    public string speaker;
    [Multiline]
    [Header("�����ı�")]
    public string content;
}
