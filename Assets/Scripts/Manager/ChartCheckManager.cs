using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    WholeObjPoolManager objPoolManager;
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
        objPoolManager = WholeObjPoolManager.Instance;

        eventCenter = EventCenter.Instance;
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<E_ChartHitState>(E_EventType.E_PlayerHit, GetPlayerHit);
        EventCenter.Instance.AddEventListener(E_EventType.E_CurrentLevelOver, LevelEnd);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<E_ChartHitState>(E_EventType.E_PlayerHit, GetPlayerHit);
        EventCenter.Instance.RemoveEventListener(E_EventType.E_CurrentLevelOver, LevelEnd);

    }

    GameObject partical;
    /// <summary>
    /// ��ҿ�����
    /// </summary>
    void GetPlayerHit(E_ChartHitState state)
    {
        switch (state)
        {
            case E_ChartHitState.Perfact:
                //��������Ч��
                eventCenter.EventTrigger(E_EventType.E_GetHitChartScore, PerfactScore);

                player.GetComponent<Player>().P.Play();
                //partical=objPoolManager.GetTargetPartical(E_ParticalType.��ҿ�������);

                break;
            case E_ChartHitState.Nice:
                //����NiceЧ��
                eventCenter.EventTrigger(E_EventType.E_GetHitChartScore, NiceScore);
                
                player.GetComponent<Player>().N.Play();
                //partical=objPoolManager.GetTargetPartical(E_ParticalType.��ҿ�������);
                break;
            case E_ChartHitState.Good:
                //����GoodЧ��
                eventCenter.EventTrigger(E_EventType.E_GetHitChartScore, GoodScore);
                
                player.GetComponent<Player>().G.Play();
                //partical=objPoolManager.GetTargetPartical(E_ParticalType.��ҿ�������);
                
                break;
            default:
                break;
        }

    }

    [Header("Ԥ�Ƶ��������ʱ��")]
    [SerializeField] private float prepareTime = 4;

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

   
    void LevelEnd() 
    {
        newMusic = false;
    }

    /// <summary>
    /// ���ݲ����������һ�β���
    /// </summary>
    /// <param name="triggerChartTimeList">������ʱ���б�</param>
    /// <returns></returns>
    public IEnumerator SetUpChartsSample(List<float> triggerChartTimeList)
    {
        currentChartIndex = 0;
        chartTimer = 0;
        newMusic = true;

        player =FindAnyObjectByType<Player>().transform;

        while (newMusic) 
        {
            chartTimer += Time.deltaTime;

            if(currentChartIndex< triggerChartTimeList.Count && 
                chartTimer > triggerChartTimeList[currentChartIndex]-prepareTime)
            {
                currentChartIndex++;

                //�ڻ������޷������ж�
                if(!PostProcessManager.Instance.isGrayWorld)
                SetUpNewChart();
                
                yield return null;
            }

            if(currentChartIndex >= triggerChartTimeList.Count)
            {
                newMusic = false;
                Debug.Log("�������");
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
        GameObject newChartObj = objPoolManager.GetChartObj();
        //�ж�Ȧ������Ҹ�����
        newChartObj.transform.SetParent(player);
        newChartObj.transform.localPosition = Vector3.zero;

        newChart = newChartObj.GetComponent<Chart>();
        //��������
        StartCoroutine(newChart.MoveToCenter());
        //StartCoroutine(newChart.MoveToCenter());

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
