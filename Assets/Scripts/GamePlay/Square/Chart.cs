using System.Collections;
using UnityEngine;

public class Chart : MonoBehaviour
{
    [Header("预计到达采样点时间")]
    [SerializeField] private float prepareTime = 4;

    [Header("总采样时长")]
    [SerializeField] private float sampleTime = 4f;

    [Header("开启采样的时刻")]
    [SerializeField] private float setUpSampleTime = 2f;

    [Header("好采样时长")]
    [SerializeField] private float goodDuration = 0.8f;
    WaitForSeconds goodDelay = null;
    [Header("优秀采样时长")]
    [SerializeField] private float niceDuration = 0.7f;
    WaitForSeconds niceDelay = null;
    [Header("完美采样时长")]
    [SerializeField] private float perfactDuration = 0.5f;

    WaitForSeconds perfactDelay = null;

    ChartCheckManager chartCheckManager;
    EventCenter eventCenter;

    WaitForSeconds sampleDelay = null;
    WholeObjPoolManager wholeObjPoolManager;

    SpriteRenderer spriteRenderer;
    void SetSpriteColor(Color color)
    {
        spriteRenderer.color = color;

    }



    bool canTrigger = true;

    private void Update()
    {
        //进入good区才会开启判定
        if (GoodState && canTrigger && PlayerInputManager.Instance.PlayerAnyAct)
        {
            canTrigger = false;

            GetPlayerHit();
        }
    }
    private void Awake()
    {
        //预缓存
        chartCheckManager ??= ChartCheckManager.Instance;
        wholeObjPoolManager = WholeObjPoolManager.Instance;
        eventCenter = EventCenter.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();

        //开启采样计时，基于设置中的节拍偏移
        sampleDelay = new WaitForSeconds(setUpSampleTime);// + chartCheckManager.currentChartOffset);
        //sampleDelay = new WaitForSeconds(1f);// + chartCheckManager.currentChartOffset);
        goodDelay = new WaitForSeconds(goodDuration);
        niceDelay = new WaitForSeconds(niceDuration);
        perfactDelay = new WaitForSeconds(perfactDuration);
    }

    //private void OnEnable()
    //{
    //    eventCenter.AddEventListener(E_EventType.E_CurrentLevelOver, LevelEnd);
    //}
    //private void OnDisable()
    //{
    //    eventCenter.RemoveEventListener(E_EventType.E_CurrentLevelOver, LevelEnd);
    //}

    //void LevelEnd() 
    //{
    //    WholeObjPoolManager.Instance.ObjReturnPool(E_ObjectPoolType.音符池,gameObject);
    //}


    //private List<IEnumerator> _sceneCoroutines = new List<IEnumerator>();

    /// <summary>
    /// 圆环向中心汇聚
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveToCenter()
    {
        canTrigger = true;
        StartCoroutine(SetUpSample());
        yield return TweenHelper.MakeLerp(Vector3.one * 5, Vector3.one * 2.34f, prepareTime, val =>
            transform.localScale = val
            );
        //缩小到精确采样点
        GetSamplePoint();
        yield return TweenHelper.MakeLerp(Vector3.one * 2.34f, Vector3.zero, prepareTime, val =>
                transform.localScale = val
        );
        wholeObjPoolManager.ObjReturnPool(E_ObjectPoolType.音符池, gameObject);
    }

    IEnumerator SetUpSample()
    {
        SetSpriteColor(Color.white);
        //基于玩家的偏移数据
        yield return sampleDelay;
        //开启采样评分
        StartCoroutine(GetSample());
    }

    bool GoodState;
    bool NiceState;
    bool PerfactState;

    IEnumerator GetSample()
    {
        SetSpriteColor(Color.blue);
        GoodState = true;
        yield return goodDelay;
        SetSpriteColor(Color.red);
        NiceState = true;
        yield return niceDelay;
        SetSpriteColor(Color.yellow);
        PerfactState = true;
        yield return perfactDelay;

        SetSpriteColor(new Color(0, 0, 0, 0));

        yield return perfactDelay;
        PerfactState = false;

        //SetSpriteColor(Color.red);
        yield return niceDelay;
        NiceState = false;
        //SetSpriteColor(Color.blue);

        yield return goodDelay;
        GoodState = false;
        //SetSpriteColor(Color.white);
        //ResetHitState();
    }

    /// <summary>
    /// 重置卡点状态,接收到玩家输入并结算后
    /// </summary>
    public void ResetChartState()
    {
        GoodState = false;
        NiceState = false;
        PerfactState = false;
    }

    /// <summary>
    /// 玩家卡上节拍，根据节拍状态结算
    /// </summary>
    public void GetPlayerHit()
    {
        if (PerfactState)
        {
            eventCenter.EventTrigger(E_EventType.E_PlayerHit, E_ChartHitState.Perfact);
            Debug.Log("完美");
        }
        else if (NiceState)
        {
            eventCenter.EventTrigger(E_EventType.E_PlayerHit, E_ChartHitState.Nice);
            Debug.Log("优秀");
        }
        else if (GoodState)
        {
            Debug.Log("好");
            eventCenter.EventTrigger(E_EventType.E_PlayerHit, E_ChartHitState.Good);
        }
    }

    /// <summary>
    /// 到达采样点
    /// </summary>
    void GetSamplePoint()
    {
        //同步播放音效
        Debug.Log("精准");
    }
}
