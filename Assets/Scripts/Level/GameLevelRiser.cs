using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameLevelRiser : MonoBehaviour
{
    [Header("�ؿ����")]
    public int LevelIndex;
    [Header("�ؿ�����ı�")]
    public TMP_Text LevelIndexText;

    [Header("���������÷�")]
    public int LevelBaseScore=1000;
    [Header("���������÷�")]
    public int LevelChartScore=2000;

    [Header("�����ʼλ��")]
    public Transform InitCamPos;
    [Header("����۽���Ļλ��")]
    public Transform FocusCamPos;
    [Header("����۽�ǰ׼��")]
    public float CamWaitTime = 3;
    [Header("����۽�ʱ��")]
    public float CameraTransDuration=2;

    [Header("�ؿ����")]
    public GameObject LevelIndexUIObj;
    [Header("����UI")]
    public GameObject ProgressUIObj;
    [Header("����")]
    public GameObject ScoreUIObj;
    [Header("����ʱ��UI")]
    public GameObject MusicTimerUIObj;
    Camera mainCam;

    [Header("���ؿ�����������Դ����Assets/streamingAssets��")]
    public string chartTimeResName;
    [Header("���ؿ�������Դ��")]
    public string musicResName;

    [Header("���ؿ�����ʱ��")]
    public float musicTime;
    ScoreController scoreController;
    GameMap map;
    notes chartTimes;



    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener(E_EventType.E_GetALevel,SetUIObjAnim);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_GetALevel,SetUIObjAnim);
    }

    void InitChartSample() 
    {
        MusicManager.Instance.PlayBKMusic(musicResName);
        scoreController.SelfInit(musicTime);
        //scoreController.SelfInit(MusicManager.Instance.PlayBKMusic(musicResName,false));
        StartCoroutine(ChartCheckManager.Instance.SetUpChartsSample(chartTimes.MusicNotes));
    }

    private void Awake()
    {
        EventCenter.Instance.EventTrigger(E_EventType.E_NewLevel);
        chartTimes = new notes(chartTimeResName);
        mainCam = Camera.main;
        scoreController = FindAnyObjectByType<ScoreController>();
        map = FindAnyObjectByType<GameMap>();

        //���ùؿ����
        LevelIndexText.text = LevelIndex.ToString();

        WholeObjPoolManager.Instance.LoadNewPool();

        StartCoroutine(CamFocusOnScreen());
    }


    IEnumerator CamFocusOnScreen() 
    {
      yield return  new WaitForSeconds(CamWaitTime);
      StartCoroutine(TweenHelper.MakeLerp(InitCamPos.localPosition, FocusCamPos.localPosition, CameraTransDuration,val=>mainCam.transform.localPosition=val));
      StartCoroutine(TweenHelper.MakeLerp(InitCamPos.localEulerAngles, FocusCamPos.localEulerAngles, CameraTransDuration,val=>mainCam.transform.localEulerAngles=val));
      yield return  new WaitForSeconds(1);
        ////��ʾ��������볡����
        StartCoroutine(UIObjScale(ScoreUIObj, Vector3.one * 0.25f));
        yield return  new WaitForSeconds(0.6f);
        //��ʾ�����볡����
        StartCoroutine(UIObjScale(LevelIndexUIObj, Vector3.one * 0.3f));
        ProgressUIObj.GetComponent<Animator>().SetTrigger("Appear");
        StartCoroutine(UIObjScale(MusicTimerUIObj, Vector3.one));
       //���ص�ͼ
       InitGameMap();
       yield return new WaitForSeconds(2);
      //��������
      InitChartSample();

    }

    void SetUIObjAnim()
    {
        ProgressUIObj.GetComponent<Animator>().SetTrigger("Idle");
    }

    IEnumerator UIObjScale(GameObject UIObj,Vector3 endValue) 
    {
       yield return TweenHelper.MakeLerp(UIObj.transform.localScale, new Vector3(0.3f, 1, 0.3f), 0.08f , val=> UIObj.transform.localScale = val);
       yield  return TweenHelper.MakeLerp(UIObj.transform.localScale, new Vector3(0.6f, 0.4f, 0.6f), 0.06f , val=> UIObj.transform.localScale = val);
       yield  return TweenHelper.MakeLerp(UIObj.transform.localScale, endValue, 0.06f , val=> UIObj.transform.localScale = val);
    }

    /// <summary>
    /// ��ʼ��Ϸ
    /// </summary>
    void InitGameMap() 
    {
        map.LoadWholeMap();
    }
}
