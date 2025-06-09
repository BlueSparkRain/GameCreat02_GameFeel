using System.Collections;
using UnityEngine;

public class Chart : MonoBehaviour
{
    [Header("Ԥ�Ƶ��������ʱ��")]
    [SerializeField] private float prepareTime = 4;

    [Header("�ܲ���ʱ��")]
    [SerializeField] private float sampleTime = 4f;

    [Header("����������ʱ��")]
    [SerializeField] private float setUpSampleTime = 2f;

    [Header("�ò���ʱ��")]
    [SerializeField] private float goodDuration = 0.8f;
    WaitForSeconds goodDelay = null;
    [Header("�������ʱ��")]
    [SerializeField] private float niceDuration = 0.7f;
    WaitForSeconds niceDelay = null;
    [Header("��������ʱ��")]
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
        //����good���ŻῪ���ж�
        if (GoodState && canTrigger && PlayerInputManager.Instance.PlayerAnyAct)
        {
            canTrigger = false;

            GetPlayerHit();
        }
    }
    private void Awake()
    {
        //Ԥ����
        chartCheckManager ??= ChartCheckManager.Instance;
        wholeObjPoolManager = WholeObjPoolManager.Instance;
        eventCenter = EventCenter.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();

        //����������ʱ�����������еĽ���ƫ��
        sampleDelay = new WaitForSeconds(setUpSampleTime);// + chartCheckManager.currentChartOffset);
        //sampleDelay = new WaitForSeconds(1f);// + chartCheckManager.currentChartOffset);
        goodDelay = new WaitForSeconds(goodDuration);
        niceDelay = new WaitForSeconds(niceDuration);
        perfactDelay = new WaitForSeconds(perfactDuration);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Բ�������Ļ��
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveToCenter()
    {
        canTrigger = true;
        StartCoroutine(SetUpSample());
        Debug.Log("��׼ǰ");
        yield return TweenHelper.MakeLerp(Vector3.one * 5, Vector3.one * 2.34f, prepareTime, val => {
            if (gameObject != null)
                transform.localScale = val;
        });
        Debug.Log("��׼��");
        //��С����ȷ������
        GetSamplePoint();
        yield return TweenHelper.MakeLerp(Vector3.one * 2.34f, Vector3.zero, prepareTime, val =>
        {
            if (gameObject != null)
                transform.localScale = val;
        });

        wholeObjPoolManager.ObjReturnPool(E_ObjectPoolType.������, gameObject);
    }

    IEnumerator SetUpSample()
    {
        SetSpriteColor(Color.white);
        //������ҵ�ƫ������
        yield return sampleDelay;
        //������������
        Debug.Log("��������ǰ");
        StartCoroutine(GetSample());
        Debug.Log("�������ֺ�");
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
    /// ���ÿ���״̬,���յ�������벢�����
    /// </summary>
    public void ResetChartState()
    {
        GoodState = false;
        NiceState = false;
        PerfactState = false;
    }

    /// <summary>
    /// ��ҿ��Ͻ��ģ����ݽ���״̬����
    /// </summary>
    public void GetPlayerHit()
    {
        if (PerfactState)
        {
            eventCenter.EventTrigger(E_EventType.E_PlayerHit, E_ChartHitState.Perfact);
            Debug.Log("����");
        }
        else if (NiceState)
        {
            eventCenter.EventTrigger(E_EventType.E_PlayerHit, E_ChartHitState.Nice);
            Debug.Log("����");
        }
        else if (GoodState)
        {
            Debug.Log("��");
            eventCenter.EventTrigger(E_EventType.E_PlayerHit, E_ChartHitState.Good);
        }
    }

    /// <summary>
    /// ���������
    /// </summary>
    void GetSamplePoint()
    {
        //ͬ��������Ч
        Debug.Log("��׼");
    }
}
