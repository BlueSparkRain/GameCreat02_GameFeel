using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 接收节拍结果并通知局内UI更新
/// </summary>
public class ChartCheckManager : MonoSingleton<ChartCheckManager>
{
    /// <summary>
    /// 玩家修改的节拍偏移量
    /// </summary>
    [Header("当前节拍偏移量")]
    public float currentChartOffset;

    //卡拍得分
    [Header("完美节拍")]
    public int PerfactScore = 50;
    [Header("优秀节拍")]
    public int NiceScore = 30;
    [Header("好节拍")]
    public int GoodScore = 20;

    WholeObjPoolManager objPoolManager;
    EventCenter eventCenter;

    Transform player;

    /// <summary>
    /// 设置全局音符偏移
    /// </summary>
    /// <param name="offset">偏移量</param>
    public void SetChartOffsetValue(float offset) 
    {
        currentChartOffset = offset;
    }

    protected override void InitSelf()
    {
        base.InitSelf();
        //预缓存
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
    /// 玩家卡上拍
    /// </summary>
    void GetPlayerHit(E_ChartHitState state)
    {
        switch (state)
        {
            case E_ChartHitState.Perfact:
                //播放完美效果
                eventCenter.EventTrigger(E_EventType.E_GetHitChartScore, PerfactScore);

                player.GetComponent<Player>().P.Play();
                //partical=objPoolManager.GetTargetPartical(E_ParticalType.玩家卡点完美);

                break;
            case E_ChartHitState.Nice:
                //播放Nice效果
                eventCenter.EventTrigger(E_EventType.E_GetHitChartScore, NiceScore);
                
                player.GetComponent<Player>().N.Play();
                //partical=objPoolManager.GetTargetPartical(E_ParticalType.玩家卡点完美);
                break;
            case E_ChartHitState.Good:
                //播放Good效果
                eventCenter.EventTrigger(E_EventType.E_GetHitChartScore, GoodScore);
                
                player.GetComponent<Player>().G.Play();
                //partical=objPoolManager.GetTargetPartical(E_ParticalType.玩家卡点完美);
                
                break;
            default:
                break;
        }

    }

    [Header("预计到达采样点时间")]
    [SerializeField] private float prepareTime = 4;

    /// <summary>
    /// 正在进行一段乐曲
    /// </summary>
    bool newMusic;
    
    float chartTimer;
    /// <summary>
    /// 当前采样点位数
    /// </summary>
    int currentChartIndex;

    /// <summary>
    /// 最新的音符
    /// </summary>
    Chart newChart;

   
    void LevelEnd() 
    {
        newMusic = false;
    }

    /// <summary>
    /// 根据采样谱面进行一段采样
    /// </summary>
    /// <param name="triggerChartTimeList">采样点时刻列表</param>
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

                //在灰屏下无法出现判定
                if(!PostProcessManager.Instance.isGrayWorld)
                SetUpNewChart();
                
                yield return null;
            }

            if(currentChartIndex >= triggerChartTimeList.Count)
            {
                newMusic = false;
                Debug.Log("生成完毕");
            }
            yield return null;
        }
    }
    /// <summary>
    /// 设置新的节奏圈
    /// </summary>
    void SetUpNewChart()
    {
        //生成新的收缩判定圈
        GameObject newChartObj = objPoolManager.GetChartObj();
        //判定圈设置玩家父对象
        newChartObj.transform.SetParent(player);
        newChartObj.transform.localPosition = Vector3.zero;

        newChart = newChartObj.GetComponent<Chart>();
        //启动音符
        StartCoroutine(newChart.MoveToCenter());
        //StartCoroutine(newChart.MoveToCenter());

    }
}

public enum E_ChartHitState
{
    /// <summary>
    /// 完美
    /// </summary>
    Perfact,
    /// <summary>
    /// 优秀
    /// </summary>
    Nice,
    /// <summary>
    /// 好
    /// </summary>
    Good,
}
