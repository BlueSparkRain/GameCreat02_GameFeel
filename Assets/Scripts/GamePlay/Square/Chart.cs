using System.Collections;
using UnityEngine;

public class Chart : MonoBehaviour
{
    [Header("Ԥ�Ƶ��������ʱ��")]
    [SerializeField] private float prepareTime = 1;

    [Header("�ܲ���ʱ��")]
    [SerializeField] private float sampleTime = 0.3f;

    [Header("����������ʱ��")]
    [SerializeField] private float setUpSampleTime = 0.8f;

    [Header("�ò���ʱ��")]
    [SerializeField] private float goodDuration= 0.15f;
    WaitForSeconds goodDelay = null;
    [Header("�������ʱ��")]
    [SerializeField] private float niceDuration = 0.1f;
    WaitForSeconds niceDelay=null;
    [Header("��������ʱ��")]
    [SerializeField] private float perfactDuration = 0.05f;
    WaitForSeconds perfactDelay = null;

    ChartCheckManager chartCheckManager;
    EventCenter eventCenter;

    WaitForSeconds sampleDelay=null;

    void Start()
    {
        //Ԥ����
        chartCheckManager ??= ChartCheckManager.Instance;
        eventCenter ??= EventCenter.Instance;

        //����������ʱ�����������еĽ���ƫ��
        sampleDelay ??= new WaitForSeconds(setUpSampleTime + chartCheckManager.currentChartOffset);
        goodDelay ??= new WaitForSeconds(goodDuration);
        niceDelay ??= new WaitForSeconds(niceDuration);
        perfactDelay ??= new WaitForSeconds(perfactDuration);
    }

    /// <summary>
    /// Բ�������Ļ��
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveToCenter()
    {
        StartCoroutine(SetUpSample());
        yield return TweenHelper.MakeLerp(Vector3.one * 3, Vector3.one, prepareTime, val => transform.localScale = val);
        //��С����ȷ������
        GetSamplePoint();
    }

    IEnumerator SetUpSample()
    {
        //������ҵ�ƫ������
        yield return sampleDelay;
        //������������
        yield return GetSample();
    }

    bool GoodState;
    bool NiceState;
    bool PerfactState;
    
    IEnumerator GetSample()
    {
        GoodState =  true;
        yield return goodDelay;
        NiceState = true;
        yield return niceDelay;
        PerfactState = true;
        yield return perfactDelay;
        ResetHitState();
    }

    /// <summary>
    /// ���ÿ���״̬,���յ�������벢�����
    /// </summary>
    public void ResetHitState() 
    {
        GoodState=false;
        NiceState=false;
        PerfactState=false;
    }

    /// <summary>
    /// ��ҿ��Ͻ��ģ����ݽ���״̬����
    /// </summary>
    public void GetPlayerHit() 
    {
        if (PerfactState)
            eventCenter.EventTrigger(E_EventType.E_PlayerHit,E_ChartHitState.Perfact);
        else if (NiceState)
            eventCenter.EventTrigger(E_EventType.E_PlayerHit,E_ChartHitState.Nice);
        else if (GoodState) 
            eventCenter.EventTrigger(E_EventType.E_PlayerHit,E_ChartHitState.Good);
    }

    /// <summary>
    /// ���������
    /// </summary>
    void GetSamplePoint()
    {
        //ͬ��������Ч
    }
}
