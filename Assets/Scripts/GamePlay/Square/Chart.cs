using System.Collections;
using UnityEngine;

public class Chart : MonoBehaviour
{
    [Header("预计到达采样点时间")]
    [SerializeField] private float prepareTime = 1;

    [Header("总采样时长")]
    [SerializeField] private float sampleTime = 0.3f;

    [Header("开启采样的时刻")]
    [SerializeField] private float setUpSampleTime = 0.8f;

    [Header("好采样时长")]
    [SerializeField] private float goodDuration= 0.15f;
    WaitForSeconds goodDelay = null;
    [Header("优秀采样时长")]
    [SerializeField] private float niceDuration = 0.1f;
    WaitForSeconds niceDelay=null;
    [Header("完美采样时长")]
    [SerializeField] private float perfactDuration = 0.05f;
    WaitForSeconds perfactDelay = null;

    ChartCheckManager chartCheckManager;
    EventCenter eventCenter;

    WaitForSeconds sampleDelay=null;

    void Start()
    {
        //预缓存
        chartCheckManager ??= ChartCheckManager.Instance;
        eventCenter ??= EventCenter.Instance;

        //开启采样计时，基于设置中的节拍偏移
        sampleDelay ??= new WaitForSeconds(setUpSampleTime + chartCheckManager.currentChartOffset);
        goodDelay ??= new WaitForSeconds(goodDuration);
        niceDelay ??= new WaitForSeconds(niceDuration);
        perfactDelay ??= new WaitForSeconds(perfactDuration);
    }

    /// <summary>
    /// 圆环向中心汇聚
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveToCenter()
    {
        StartCoroutine(SetUpSample());
        yield return TweenHelper.MakeLerp(Vector3.one * 3, Vector3.one, prepareTime, val => transform.localScale = val);
        //缩小到精确采样点
        GetSamplePoint();
    }

    IEnumerator SetUpSample()
    {
        //基于玩家的偏移数据
        yield return sampleDelay;
        //开启采样评分
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
    /// 重置卡点状态,接收到玩家输入并结算后
    /// </summary>
    public void ResetHitState() 
    {
        GoodState=false;
        NiceState=false;
        PerfactState=false;
    }

    /// <summary>
    /// 玩家卡上节拍，根据节拍状态结算
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
    /// 到达采样点
    /// </summary>
    void GetSamplePoint()
    {
        //同步播放音效
    }
}
