using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���ս��Ľ����֪ͨ����UI����
/// </summary>
public class ChartCheckManager : MonoSingleton<ChartCheckManager>
{
    /// <summary>
    /// ����޸ĵĽ���ƫ����
    /// </summary>
    [Header("��ǰ����ƫ����")]
    public float currentChartOffset;

    //���ĵ÷�
    [Header("��������")]
    public int PerfactScore = 50;
    [Header("�������")]
    public int NiceScore = 30;
    [Header("�ý���")]
    public int GoodScore = 20;

    ChartObjPoolManager chartPoolInstance;
    EventCenter eventCenter;

    Transform player;

    /// <summary>
    /// ����ȫ������ƫ��
    /// </summary>
    /// <param name="offset">ƫ����</param>
    public void SetChartOffsetValue(float offset) 
    {
        currentChartOffset = offset;
    }

    protected override void InitSelf()
    {
        base.InitSelf();
        //Ԥ����
        chartPoolInstance=ChartObjPoolManager.Instance;
        eventCenter = EventCenter.Instance;
    }



    private void OnEnable()
    {
        eventCenter.AddEventListener<E_ChartHitState>(E_EventType.E_PlayerHit, GetPlayerHit);
    }

    private void OnDisable()
    {
        eventCenter.RemoveEventListener<E_ChartHitState>(E_EventType.E_PlayerHit, GetPlayerHit);
    }

    /// <summary>
    /// ��ҿ�����
    /// </summary>
    void GetPlayerHit(E_ChartHitState state)
    {
        switch (state)
        {
            case E_ChartHitState.Perfact:
                eventCenter.EventTrigger(E_EventType.E_GetHitScore, PerfactScore);
                break;
            case E_ChartHitState.Nice:
                eventCenter.EventTrigger(E_EventType.E_GetHitScore, NiceScore);
                break;
            case E_ChartHitState.Good:
                eventCenter.EventTrigger(E_EventType.E_GetHitScore, GoodScore);
                break;
            default:
                break;
        }
    }

    [Header("Ԥ�Ƶ��������ʱ��")]
    [SerializeField] private float prepareTime = 1;

    /// <summary>
    /// ���ڽ���һ������
    /// </summary>
    bool newMusic;
    
    float chartTimer;
    /// <summary>
    /// ��ǰ������λ��
    /// </summary>
    int currentChartIndex;

    /// <summary>
    /// ���µ�����
    /// </summary>
    Chart newChart;

    /// <summary>
    /// ����һ���µ�����
    /// </summary>
    /// <param name="triggerChartTimeList">������ʱ���б�</param>
    /// <returns></returns>
    public IEnumerator SetUpNewMusic(List<int> triggerChartTimeList)
    {
        currentChartIndex = 0;
        newMusic = true;

        player??=FindAnyObjectByType<Player>().transform;

        while (newMusic) 
        {
            chartTimer += Time.time;
            if(chartTimer > triggerChartTimeList[currentChartIndex]-prepareTime)
            {
                SetUpNewChart();
            }
            yield return null;
        }
    }

    /// <summary>
    /// �����µĽ���Ȧ
    /// </summary>
    void SetUpNewChart()
    {
        //�����µ������ж�Ȧ
        GameObject newChartObj = chartPoolInstance.GetChartInstnceFromPool();
        //�ж�Ȧ������Ҹ�����
        newChartObj.transform.SetParent(player);
        newChartObj.transform.localPosition = Vector3.zero;

        newChart = newChartObj.GetComponent<Chart>();
        //��������
        StartCoroutine(newChart.MoveToCenter());
    }
}

public enum E_ChartHitState
{
    /// <summary>
    /// ����
    /// </summary>
    Perfact,
    /// <summary>
    /// ����
    /// </summary>
    Nice,
    /// <summary>
    /// ��
    /// </summary>
    Good,
}
